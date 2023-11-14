﻿using Daemon.DataAccess.DataStore;


namespace SmartEstimate.Models;

public enum QuoteStatus
{
    Draft,
    InReview,
    Approved,
}

public struct Quote : IDbModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }



    public string Name { get; set; }
    public QuoteStatus Status  { get; set; }
    public string SalesAssociate { get; set; }
    public string ContactInfo { get; set; }
    public ShippingAddress  DealerAddress { get; set; }
    public ShippingAddress  CustomerAddress { get; set; }
    public bool IsPickup { get; set; }
    public bool IsDelivery { get; set; }
    public bool IsDealer { get; set; }
    public bool IsApartment { get; set; }
    public bool IsMultiLevel { get; set; }
    public bool IsFreightForwarder { get; set; }
    public string CatelogPdfUrl { get; set; }
    public FileDocument[] Files { get; set; }

    public List<Room> Rooms { get; set; }
}

