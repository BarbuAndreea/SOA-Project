import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IRoom } from 'src/app/models/IRoom';
import { RoomService } from '../room.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent implements OnInit {
  roomForm!: FormGroup;
  errorMessage: string = '';
  rooms: IRoom[] = [];
  backendErrorMessage: string = '';
  isFullDescriptionVisibile: boolean = false;
  selectedRoom: number | undefined = -1;
  room!: IRoom;

  constructor(private roomService: RoomService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.getAllRooms();
    this.roomForm = this.formBuilder.group({
      nameInput: ['', Validators.required],
      equipmentInput: ['', Validators.required]
    })
  }

  toggleFullDescription() {
    this.isFullDescriptionVisibile = !this.isFullDescriptionVisibile;
  }

  getAllRooms(): void {
    this.roomService.getRoomsByClinic()
      .subscribe({
        next: (res) => {
          this.rooms = res;
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  get formFields() { return this.roomForm.controls; }

  onSubmit() {
    if (this.roomForm.invalid) {
      return;
    }

    this.room = {
      name: this.formFields['nameInput'].value,
      medicalEquipment: this.formFields['equipmentInput'].value
    }

    if(this.selectedRoom == -1)
    {
      this.addRoom(this.room);
    }
    else
    {
      this.room.id = this.selectedRoom,
      this.roomService.updateRoom(this.room)
      .subscribe({
        next: () => {
          this.errorMessage = '';
          this.getAllRooms();
          this.roomForm.reset();
          this.selectedRoom = -1;
        },
        error: () => {
          this.errorMessage = 'An error occured and the office location could not be added! Please try again!';
        }
      })
    }
  }

  addRoom(room: IRoom): void {
    this.roomService.addRoom(room)
      .subscribe({
        next: () => {
          this.errorMessage = '';
          this.getAllRooms();
          this.roomForm.reset();
        },
        error: () => {
          this.errorMessage = 'An error occured and the office location could not be added! Please try again!';
        }
      })
  }

  onDeleteRoom(id: number | undefined){
    this.roomForm.reset();
    if(confirm("Are you sure to delete this room?")) {
      this.roomService.deleteRoom(id)
        .subscribe({
          next: () => {
            this.getAllRooms();
          },
          error: (e) => {
            console.log(e)
          }
      })
    }
  }

  populateForm(room: IRoom){
    this.selectedRoom = room.id;
    this.formFields['nameInput'].setValue(room.name);
    this.formFields['equipmentInput'].setValue(room.medicalEquipment);
  }

  cancelAddRoom() {
    this.roomForm.reset();
  }
}
