# Errors

This article covers how to resolve common errors on the Timestamp Generator Gen3.

## COM port errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist."**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB](ports-connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: In Bonsai, running the workflow throws an error "Access to the port `ComX` is denied"**

A: Only one interface connection to the Timestamp Generator Gen3 can be opened at one time. Check that multiple instances of Bonsai are not running. Sometimes, the port can also be locked by a program that did not terminate correctly. Restarting the computer fixes it.

## Device errors

**Q: In Bonsai, setting the clock throws an error "The device reported an erroneous write command. Payload: 0, Address: 8, Type: U32."**

A: The device clock is locked, which protects it from being reset by mistake. Unlock the clock by holding the [OPTION](ports-connections.md#front-panel) button for about 3 seconds, so that the **LOCK** LED turns off, and then [set the clock](set-clock.md) again.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.TimestampGeneratorGen3.Device
