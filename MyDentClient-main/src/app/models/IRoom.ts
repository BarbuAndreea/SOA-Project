import { IClinic } from "./IClinic"

export interface IRoom {
        id?: number
        name: string
        medicalEquipment: string
        clinicId?: number
}