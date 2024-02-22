import { Specializations } from "./IMedic"

export enum UserRole {
    SuperAdmin = 0,
    ClinicAdmin = 1,
    Medic = 2,
    Patient = 3
}
export interface IUser {
    id?: number
    email: string
    firstName: string
    lastName: string
    age: number
    phoneNumber: string
    password: string
    token?: string
    role: UserRole
}

export interface IUserToAdd {
    email: string
    firstName: string
    lastName: string
    age: number
    phoneNumber: string
    password: string
    role: UserRole
    specialization: Specializations,
    clinicId: number | undefined
}