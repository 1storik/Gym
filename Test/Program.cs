//using Gym.Models;
//using Gym.Repository;

class Program
{
    static void Main()
    {
        // Тело метода Main
    }
}
//class Program
//{
//    static void Main()
//    {
//        TestManyToManyRelationship();

//        Console.WriteLine("Tests completed. Press any key to exit.");
//        Console.ReadKey();
//    }

//    static void TestManyToManyRelationship()
//    {
//        ClientRepository clientRepository = new ClientRepository("D:\\\\Project\\\\Visual Studio\\\\Gym\\\\Gym\\\\Data\\\\Client.json");
//        MembershipRepository membershipRepository = new MembershipRepository("D:\\Project\\Visual Studio\\Gym\\Gym\\Data\\Membership.json");

//        //var m = membershipRepository.FirstOrDefault(m => m.MembershipType == "Gold");
//        //var c = clientRepository.FirstOrDefault();
//        //membershipRepository.RemoveClientToMembership(m, c);

//        var client1 = new Client
//        {
//            FirstName = "John",
//            LastName = "Doe"
//        };

//        var client2 = new Client
//        {
//            FirstName = "Dima",
//            LastName = "Fen"
//        };

//        clientRepository.Add(client1);
//        clientRepository.Add(client2);


//        var membership1 = new Membership
//        {
//            MembershipType = "Gold",
//            Coach = "Personal Trainer",
//            Price = 100
//        };

//        var membership2 = new Membership
//        {
//            MembershipType = "Silver",
//            Coach = "Fitness Instructor",
//            Price = 50
//        };

//        membershipRepository.Add(membership1);
//        membershipRepository.Add(membership2);

//        var newClient1 = clientRepository.FirstOrDefault(c => c.FirstName == "John");
//        var newClient2 = clientRepository.FirstOrDefault(c => c.FirstName == "Dima");
//        var newMembership1 = membershipRepository.FirstOrDefault(m => m.MembershipType == "Platinum");

//        membershipRepository.AddClientToMembership(newMembership1, newClient1, DateTime.Now, DateTime.Now.AddYears(1));
//        membershipRepository.AddClientToMembership(newMembership1, newClient2, DateTime.Now, DateTime.Now.AddYears(1));


//        Console.WriteLine("TestManyToManyRelationship PASSED");
//    }
//}
