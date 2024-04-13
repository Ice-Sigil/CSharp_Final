public class Shopkeeper : NPCInt{

    _isShopkeeper = true;
    _isHuman = true;
    _isHostile = false;
    _discoveryDialogue = "Good to see you. Lots of things for sale here. Take a look.";

    _HP = 100; //Temp, will be changed for game balancing and/or difficulty

    _possibleDialogue[0] = "Is this really what you want to do? Fine, then. Die like the rest.";
    _possibleDialogue[1] = "Fool. You'll get what you deserve.";
    _possibleDialogue[2] = "You should have never come down here.";
    _possibleDialogue[3] = "May the gods let you die quickly.";
    _possibleDialogue[4] = "Wretched filth. I'll put you where you belong.";


}
