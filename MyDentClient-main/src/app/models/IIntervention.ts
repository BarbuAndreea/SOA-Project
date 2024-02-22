import { IMedic } from "./IMedic"

export enum ToothEnum {
    Undefined = 0,
    T1 = 1,
    T2 = 2,
    T3 = 3,
    T4 = 4,
    T5 = 5,
    T6 = 6,
    T7 = 7,
    T8 = 8,
    T9 = 9,
    T10 = 10,
    T11 = 11,
    T12 = 12,
    T13 = 13,
    T14 = 14,
    T15 = 15,
    T16 = 16,
    T17 = 17,
    T18 = 18,
    T19 = 19,
    T20 = 20,
    T21 = 21,
    T22 = 22,
    T23 = 23,
    T24 = 24,
    T25 = 25,
    T26 = 26,
    T27 = 27,
    T28 = 28,
    T29 = 29,
    T30 = 30,
    T31 = 31,
    T32 = 32
}

export interface IIntervention {
    id?: number
    patientId: number
    medicId: number
    name: string
    recommendation?: string
    description: string
    teeth: ToothEnum
    price: number
    date: Date
    medic?: IMedic
}