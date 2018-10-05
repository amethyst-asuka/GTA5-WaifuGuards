## Sequence Diagram

```
    Player      Server
Ping | --------> | Create a new guid, checksum code, empty user object
     | <-------- | Response guid and checksum code
Login| --------> | Check checksum with previous, broadcast other player and response a new checksum code
```