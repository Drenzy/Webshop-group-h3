import { Address } from "./address";
import { Customer } from "./customer";
import { OrderItem } from "./orderitem";
import { Status } from "./status";

export interface Order{
    id: number,
    customerId?: number,
    addressId?: number,
    orderDate?: Date,
    statusId?: number,
    status?: Status,
    orderItems?: OrderItem[],
    address?: Address,
    customer?: Customer 
}