namespace AzureSqlTest.ConsoleTests
{
    public class Utility
    {
        public static string? ParseString(string prompt, bool optional = false)
        {
            string newPrompt = $"{prompt}{(optional ? " (optional)" : "")}:";
            Console.Write(newPrompt);
            string? value = Console.ReadLine();
            if (optional) return value;

            while (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Invalid input. Please enter a value.");
                Console.Write(newPrompt);
                value = Console.ReadLine();
            }

            return value;
        }

        public static int ParseInt(string prompt)
        {
            int value = 0;
            while (value <= 0)
            {
                Console.Write(prompt);
                if (!int.TryParse(Console.ReadLine(), out value))
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer.");
                }
            }

            return value;
        }

        internal static async Task RunAsync(AddressCRUD addressCRUD)
        {
            while (true)
            {
                Console.WriteLine("1. Create a new address");
                Console.WriteLine("2. Read an address");
                Console.WriteLine("3. Update an address");
                Console.WriteLine("4. Delete an address");
                Console.WriteLine("5. Exit");
                int choice = Utility.ParseInt("Enter your choice: ");
                string? city = string.Empty;
                string? stateProvince = string.Empty;
                string? countryRegion = string.Empty;
                string? postalCode = string.Empty;
                string? addressLine1 = string.Empty;
                string? addressLine2 = null;
                int addressId = 0;
                switch (choice)
                {
                    case 1:
                        city = Utility.ParseString("Enter the city: ");
                        stateProvince = Utility.ParseString("Enter the state/province: ");
                        countryRegion = Utility.ParseString("Enter the country/region: ");
                        postalCode = Utility.ParseString("Enter the postal code: ");
                        addressLine1 = Utility.ParseString("Enter the address line 1: ");
                        addressLine2 = Utility.ParseString("Enter the address line 2 (optional): ", optional: true);
                        await addressCRUD.TryCreateAsync(city!, stateProvince!, countryRegion!, postalCode!, addressLine1!, addressLine2!);
                        break;
                    case 2:
                        addressId = Utility.ParseInt("Enter the address ID: ");
                        await addressCRUD.TryReadAsync("AddressID", addressId.ToString());
                        break;
                    case 3:
                        addressId = Utility.ParseInt("Enter the address ID: ");
                        city = Utility.ParseString("Enter the city: ", true);
                        stateProvince = Utility.ParseString("Enter the state/province: ", true);
                        countryRegion = Utility.ParseString("Enter the country/region: ", true);
                        postalCode = Utility.ParseString("Enter the postal code: ", true);
                        addressLine1 = Utility.ParseString("Enter the address line 1: ", true);
                        addressLine2 = Utility.ParseString("Enter the address line 2: ", optional: true);
                        await addressCRUD.TryUpdateAsync(addressId, city!, stateProvince!, countryRegion!, postalCode!, addressLine1!, addressLine2!);
                        break;
                    case 4:
                        addressLine1 = Utility.ParseString("Enter the address ID: ");
                        await addressCRUD.TryDeleteAsync(addressId);
                        break;
                    case 5:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
    }
}