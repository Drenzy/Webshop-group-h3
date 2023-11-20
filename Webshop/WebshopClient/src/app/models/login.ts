import { Role } from "./role"
import { Customer } from "./customer"
export interface Login {
    id: number,
    userName: string,
    password?: string,
    email: string,
    role?: Role,
    token?: string,
    customer?: Customer
}