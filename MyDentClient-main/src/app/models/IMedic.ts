import { IClinic } from "./IClinic";
import { IHoliday } from "./IHoliday";
import { IUser } from "./IUser";

export enum Specializations {
    Undefined = 0,
    Dental_Public_Health = 1,
    Endodontics = 2,
    Oral_and_Maxillofacial_Surgery = 3,
    Oral_Medicine_and_Pathology = 4,
    Oral_Maxillofacial_Radiology = 5,
    Orthodontics_and_Dentofacial_Orthopedics = 6,
    Pediatric_Dentistry = 7,
    Periodontics = 8,
    Prosthodontics = 9
}

export interface IMedic {
    id?: number
    userM: IUser
    specialization: Specializations
    clinicId: number
    startWorkingHour: Date
    endWorkingHour: Date
    holidays?: IHoliday[]
}