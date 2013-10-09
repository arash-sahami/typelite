﻿

 


declare module Eshop {
enum CustomerKind {
  Corporate = 1,
  Individual = 2
}
interface Customer {
  Name: string;
  Email: string;
  VIP: boolean;
  Kind: Eshop.CustomerKind;
  Orders: Eshop.Order[];
}
interface Order {
  Products: Eshop.Product[];
  TotalPrice: number;
  Created: Date;
}
interface Product {
  Name: string;
  Price: number;
  ID: System.Guid;
}
}
declare module System {
interface Guid {
}
}
