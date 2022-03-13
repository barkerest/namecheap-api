using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.Tests.Commands.Domains;

#pragma warning disable xUnit2013

public class Create_Should
{
    private readonly ITestOutputHelper _output;

    public Create_Should(ITestOutputHelper output)
    {
        _output = output;
    }

    #region CreateValidCommand

    private Create CreateValidCommand()
    {
        // default blank values are commented out.
        return new Create(Config.ApiConfigWithLogging(_output))
        {
            DomainName = "onebarker-dev.com",
            YearsToRegister = 2,
            //PromotionCode = "",
            Registrant =
            {
                //OrganizationName = "",
                //JobTitle = "",
                FirstName = "John",
                LastName = "Doe",
                Address1 = "1 Center Court",
                //Address2 = "",
                City = "Cleveland",
                StateOrProvince = "OH",
                //StateOrProvinceChoice = "",
                PostalCode = "44115",
                Country = "US",
                Phone = "+1.8005554321",
                //PhoneExt = "",
                //Fax = "",
                EmailAddress = "j.doe@example.com",
            },
            Tech = 
            {
                //OrganizationName      = "",
                //JobTitle              = "",
                FirstName             = "John",
                LastName              = "Doe",
                Address1              = "1 Center Court",
                //Address2              = "",
                City                  = "Cleveland",
                StateOrProvince       = "OH",
                //StateOrProvinceChoice = "",
                PostalCode            = "44115",
                Country               = "US",
                Phone                 = "+1.8005554321",
                //PhoneExt              = "",
                //Fax                   = "",
                EmailAddress          = "j.doe@example.com",
            },
            Admin =
            {
                //OrganizationName      = "",
                //JobTitle              = "",
                FirstName             = "John",
                LastName              = "Doe",
                Address1              = "1 Center Court",
                //Address2              = "",
                City                  = "Cleveland",
                StateOrProvince       = "OH",
                //StateOrProvinceChoice = "",
                PostalCode            = "44115",
                Country               = "US",
                Phone                 = "+1.8005554321",
                //PhoneExt              = "",
                //Fax                   = "",
                EmailAddress          = "j.doe@example.com",
            },
            AuxBilling = 
            {
                //OrganizationName      = "",
                //JobTitle              = "",
                FirstName             = "John",
                LastName              = "Doe",
                Address1              = "1 Center Court",
                //Address2              = "",
                City                  = "Cleveland",
                StateOrProvince       = "OH",
                //StateOrProvinceChoice = "",
                PostalCode            = "44115",
                Country               = "US",
                Phone                 = "+1.8005554321",
                //PhoneExt              = "",
                //Fax                   = "",
                EmailAddress          = "j.doe@example.com",
            },
            //Billing = null,
            //IdnCode = "",
            //ExtendedAttributes = null,
            //Nameservers = Array.Empty<string>(),
            //AddFreeWhoisGuard = null,
            //EnableWhoisGuard = null,
            //PremiumPrice = null,
            //EapFee = null,
        };
    }
    
    #endregion

    #region CreateValidContact
    private Contact CreateValidContact()
    {
        return new Contact()
        {
            //OrganizationName = "",
            //JobTitle = "",
            FirstName = "John",
            LastName  = "Doe",
            Address1  = "1 Center Court",
            //Address2 = "",
            City            = "Cleveland",
            StateOrProvince = "OH",
            //StateOrProvinceChoice = "",
            PostalCode = "44115",
            Country    = "US",
            Phone      = "+1.8005554321",
            //PhoneExt = "",
            //Fax = "",
            EmailAddress = "j.doe@example.com",
        };
    }
    #endregion
    
    #region InvalidLongStrings
    
    private const string Invalid21CharString  = "A-21-char-string-zzzz";
    private const string Invalid51CharString  = "A-51-char-string-zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";
    private const string Invalid71CharString  = "A-71-char-string-zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";
    private const string Invalid256CharString = "A-256-char-string-zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz1zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz2zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz123456";
    
    #endregion


    [Fact]
    public void BeValidForTesting()
    {
        var valid = CreateValidCommand().IsValid(out var errors);

        if (!valid)
        {
            _output.WriteLine("Errors:\n  " + string.Join("\n  ", errors));
            throw new ApplicationException("Configuration is not valid for testing.");
        }
        
        _output.WriteLine("The configuration is valid.");
    }

    //[Fact]
    public void Execute()
    {
        var cmd = CreateValidCommand();
        Assert.True(cmd.IsValid());
        var response = cmd.GetResult();
        Assert.True(response.Result.Registered);
    }

    private void RunRejectTest(Action<Create> setBadValue, params string[] propNames)
    {
        var config = CreateValidCommand();
        setBadValue(config);
        var valid = config.IsValid(out var errors);
        Assert.False(valid, string.Join(", ", propNames) + " should be invalid");
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(propNames.Length, errors.Length);
        foreach (var propName in propNames)
        {
            Assert.Contains(errors, x => x.Contains(propName));
        }
    }

    private void RunContactRejectTest(Action<Contact> setBadValue, params string[] propNames)
    {
        RunRejectTest(config => setBadValue(config.Registrant), propNames.Select(x => "Registrant." + x).ToArray());
        RunRejectTest(config => setBadValue(config.Tech), propNames.Select(x => "Tech." + x).ToArray());
        RunRejectTest(config => setBadValue(config.Admin), propNames.Select(x => "Admin." + x).ToArray());
        RunRejectTest(config => setBadValue(config.AuxBilling), propNames.Select(x => "AuxBilling." + x).ToArray());
        RunRejectTest(config =>
        {
            config.Billing = CreateValidContact();
            setBadValue(config.Billing);
        }, propNames.Select(x => "Billing." + x).ToArray());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidDomainName(string domainName)
        => RunRejectTest(config => config.DomainName = domainName, "DomainName");

    [Theory]
    [InlineData(Invalid21CharString)]
    public void RejectInvalidPromotionCode(string promoCode)
        => RunRejectTest(config => config.PromotionCode = promoCode, "PromotionCode");

    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidOrganizationName(string orgName)
        => RunContactRejectTest(contact => contact.OrganizationName   = orgName, "OrganizationName");

    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidJobTitle(string title)
        => RunContactRejectTest(contact => contact.JobTitle = title, "JobTitle");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidFirstName(string firstName)
        => RunContactRejectTest(contact => contact.FirstName = firstName, "FirstName");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidLastName(string lastName)
        => RunContactRejectTest(contact => contact.LastName = lastName, "LastName");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidAddress1(string address)
        => RunContactRejectTest(contact => contact.Address1 = address, "Address1");
    
    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidAddress2(string address)
        => RunContactRejectTest(contact => contact.Address2 = address, "Address2");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidCity(string city)
        => RunContactRejectTest(contact => contact.City = city, "City");
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidStateProvince(string stateProvince)
        => RunContactRejectTest(contact => contact.StateOrProvince = stateProvince, "StateOrProvince");

    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidStateProvinceChoice(string stateProvince)
        => RunContactRejectTest(contact => contact.StateOrProvinceChoice = stateProvince, "StateOrProvinceChoice");
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPostalCode(string postalCode)
        => RunContactRejectTest(contact => contact.PostalCode = postalCode, "PostalCode");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidCountry(string country)
        => RunContactRejectTest(contact => contact.Country = country, "Country");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPhone(string phone)
        => RunContactRejectTest(contact => contact.Phone = phone, "Phone");

    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPhoneExt(string phone)
        => RunContactRejectTest(contact => contact.PhoneExt = phone, "PhoneExt");

    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidFax(string fax)
        => RunContactRejectTest(contact => contact.Fax = fax, "Fax");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidEmailAddress(string address)
        => RunContactRejectTest(contact => contact.EmailAddress = address, "EmailAddress");
    
    
}
