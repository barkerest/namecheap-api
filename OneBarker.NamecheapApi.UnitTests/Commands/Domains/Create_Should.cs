using System;
using System.Linq;
using System.Linq.Expressions;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

#pragma warning disable xUnit2013

public class Create_Should : CommonTestBase<Create>
{
    public Create_Should(ITestOutputHelper output) : base(output)
    {
        
    }

    #region CreateValidCommand

    protected override Create CreateValidCommand()
    {
        // default blank values are commented out.
        return new Create(GoodConfig)
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


    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());
    
    /*
    [Fact]
    public void Execute()
    {
        var cmd = CreateValidCommand();
        Assert.True(cmd.IsValid());
        var response = cmd.GetResult();
        Assert.True(response.Result.Registered);
    }
    */

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid71CharString)]
    public void RejectInvalidDomainName(string domainName)
        => TestInvalidOption(c => c.DomainName, domainName);

    [Theory]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("Hello")]
    [InlineData("Hello.World")]
    [InlineData(Valid70CharString)]
    public void PermitValidDomainName(string domainName)
        => TestValidOption(c => c.DomainName, domainName);
    
    [Theory]
    [InlineData(Invalid21CharString)]
    public void RejectInvalidPromotionCode(string promoCode)
        => TestInvalidOption(c => c.PromotionCode, promoCode);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid20CharString)]
    public void PermitValidPromotionCode(string promoCode)
        => TestValidOption(x => x.PromotionCode, promoCode);

    private void TestInvalidOptionsForContact<TValue>(Expression<Func<Contact, TValue>> property, TValue value)
    {
        TestInvalidOptionsFor(
            property,
            c => c.Billing = null,
            value,
            c => c.Registrant,
            c => c.Tech,
            c => c.Admin,
            c => c.AuxBilling
        );
        TestInvalidOptionsFor(
            property,
            c => c.Billing = CreateValidContact(),
            value,
            c => c.Billing
        );
    }
    
    private void TestValidOptionsForContact<TValue>(Expression<Func<Contact, TValue>> property, TValue value)
    {
        TestValidOptionsFor(
            property,
            c => c.Billing = null,
            value,
            c => c.Registrant,
            c => c.Tech,
            c => c.Admin,
            c => c.AuxBilling
        );
        TestValidOptionsFor(
            property,
            c => c.Billing = CreateValidContact(),
            value,
            c => c.Billing
        );
    }

    

    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidOrganizationName(string orgName)
        => TestInvalidOptionsForContact(c => c.OrganizationName, orgName);


    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidOrganizationName(string orgName)
        => TestValidOptionsForContact(x => x.OrganizationName, orgName);
    
    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidJobTitle(string title)
        => TestInvalidOptionsForContact(c => c.JobTitle, title);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidJobTitle(string title)
        => TestValidOptionsForContact(x => x.JobTitle, title);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidFirstName(string firstName)
        => TestInvalidOptionsForContact(c => c.FirstName, firstName);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidFirstName(string firstName)
        => TestValidOptionsForContact(x => x.FirstName, firstName);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidLastName(string lastName)
        => TestInvalidOptionsForContact(c => c.LastName, lastName);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidLastName(string lastName)
        => TestValidOptionsForContact(c => c.LastName, lastName);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidAddress1(string address)
        => TestInvalidOptionsForContact(c => c.Address1, address);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidAddress1(string address)
        => TestValidOptionsForContact(x => x.Address1, address);
    
    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidAddress2(string address)
        => TestInvalidOptionsForContact(c => c.Address2, address);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidAddress2(string address)
        => TestValidOptionsForContact(x => x.Address2, address);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidCity(string city)
        => TestInvalidOptionsForContact(c => c.City, city);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidCity(string city)
        => TestValidOptionsForContact(c => c.City, city);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidStateProvince(string stateProvince)
        => TestInvalidOptionsForContact(c => c.StateOrProvince, stateProvince);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidStateProvince(string stateProvince)
        => TestValidOptionsForContact(c => c.StateOrProvince, stateProvince);
    
    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidStateProvinceChoice(string stateProvince)
        => TestInvalidOptionsForContact(c => c.StateOrProvinceChoice, stateProvince);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidStateProvinceChoice(string stateProvince)
        => TestValidOptionsForContact(c => c.StateOrProvinceChoice, stateProvince);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPostalCode(string postalCode)
        => TestInvalidOptionsForContact(c => c.PostalCode, postalCode);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidPostalCode(string postalCode)
        => TestValidOptionsForContact(c => c.PostalCode, postalCode);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidCountry(string country)
        => TestInvalidOptionsForContact(c => c.Country, country);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidCountry(string country)
        => TestValidOptionsForContact(c => c.Country, country);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPhone(string phone)
        => TestInvalidOptionsForContact(c => c.Phone, phone);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidPhone(string phone)
        => TestValidOptionsForContact(c => c.Phone, phone);
    
    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidPhoneExt(string phone)
        => TestInvalidOptionsForContact(c => c.PhoneExt, phone);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidPhoneExt(string phone)
        => TestValidOptionsForContact(c => c.PhoneExt, phone);
    
    [Theory]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidFax(string fax)
        => TestInvalidOptionsForContact(c => c.Fax, fax);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid50CharString)]
    public void PermitValidFax(string fax)
        => TestValidOptionsForContact(c => c.Fax, fax);
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidEmailAddress(string address)
        => TestInvalidOptionsForContact(c => c.EmailAddress, address);

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid255CharString)]
    public void PermitValidEmailAddress(string address)
        => TestValidOptionsForContact(c => c.EmailAddress, address);
}
