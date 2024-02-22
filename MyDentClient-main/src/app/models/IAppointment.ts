import { IMedic } from "./IMedic"
import { IPatient } from "./IPatient"

export enum AppointmentStatus {
    Unstarted,
    Active,
    Finished,
    Unsolved
}

export interface IAppointment {
    id?: number
    patientId: number
    roomId: number
    medicId: number
    name: string
    startDate: Date
    endDate: Date
    status: AppointmentStatus
    medic?: IMedic
    patient?: IPatient
    roomName?: string
}