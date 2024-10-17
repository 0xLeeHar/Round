using Newtonsoft.Json;

namespace Round.Common.Domain;

[JsonConverter(typeof(BankAccountAddressConverter))]
public record BankAccountAddress
{
    // This is taken from India's UPI framework, its great and worth a read!
    // Addresses are in the format of `accountNumber@routingCode.scheme`
    // E.g. UK account number and sort code would be: 12345678@206512.payuk
    // Bic and IBAN would be: GB29NWBK60161331926819@HBUKGB4B.swift
    public required string Value { get; init; }
    
    // TODO: Add functions here for handling address identifiers / schemes
}