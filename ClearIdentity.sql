﻿DELETE FROM [User]
DBCC CHECKIDENT('User', RESEED, 0)