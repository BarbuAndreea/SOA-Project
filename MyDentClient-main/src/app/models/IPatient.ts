import { IClinic } from "./IClinic";
import { IIntervention } from "./IIntervention";
import { IRadiography } from "./IRadiography";
import { IUser } from "./IUser";

export interface IPatient {
    id: number
    userP: IUser
    interventions: IIntervention[]
    clinics: IClinic[]
    radiographies: IRadiography[]
}