using Exiled.API.Interfaces;
using SerpentHands.Items;
using SerpentHands.Roles;

namespace SerpentHands
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        public GeneralSettings GeneralSettings { get; set; } = new();
        public SquadRoles SquadRoles { get; set; } = new();
        public SquadKeycards SquadKeycards { get; set; } = new();
    }

    public class SquadKeycards
    {
        public KeycardLeader KeycardLeader { get; set; } = new();
        public KeycardEagle KeycardEagle { get; set; } = new();
        public KeycardInitiator KeycardInitiator { get; set; } = new();
        public KeycardJumper KeycardJumper { get; set; } = new();
        public KeycardSupport KeycardSupport { get; set; } = new();
    }
    
    public class SquadRoles
    {
        public Leader Leader { get; set; } = new();
        public Initiator Initiator { get; set; } = new();
        public Eagle Eagle { get; set; } = new();
        public Jumper Jumper { get; set; } = new();
        public Support Support { get; set; } = new();
    }
}