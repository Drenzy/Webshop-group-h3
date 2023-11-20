import { Role } from "./role";

export interface SignIn {
    loginId: number,
    customerId?: number,
    addressId?: number,
    role: Role,
    token: string,
}