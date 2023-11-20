import { Address } from './address';
import { Login } from './login';
export interface Customer{
    id: number,
    name?: string,
    phoneNr?: string,
    addressId?: number,
    loginId?: number,
    logins?: Login,
    addresses?: Address
}