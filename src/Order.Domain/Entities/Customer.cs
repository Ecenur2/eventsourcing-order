﻿namespace Order.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; }
}