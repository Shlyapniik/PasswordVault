# PasswordVault

PasswordVault is a cross-platform password manager built with C# and .NET MAUI.

The application allows users to securely store, search, generate, edit and delete password entries. Sensitive entry data is encrypted before being stored in SQLite.

## Features

* Master password protected vault
* Add, edit and delete password entries
* Search by title, username and website
* Password generator
* Copy passwords to clipboard
* Lock and unlock the vault
* Persistent local storage using SQLite
* Existing database migration
* AES-GCM encryption
* PBKDF2 key derivation
* Cryptographically secure random number generation
* Cross-platform .NET MAUI application

## Security

The master password is never stored in the database.

When the vault is created, a random salt is generated. The encryption key is derived from the master password using PBKDF2.

```text
Master Password
       │
       ▼
PBKDF2 + Random Salt
       │
       ▼
32-byte Encryption Key
       │
       ▼
AES-GCM
       │
       ▼
Encrypted Data
       │
       ▼
SQLite
```

Sensitive password entry fields are serialized into a JSON object and encrypted as a single AES-GCM payload.

The following fields are encrypted:

* Username
* Password
* Website
* Notes

The entry title is stored separately in plaintext to allow searching without decrypting every record.

SQLite stores the following information for password entries:

* Entry ID
* Title
* Encrypted data

The application uses cryptographically secure random values for:

* PBKDF2 salts
* AES-GCM nonces
* Password generation
* Cryptographic shuffling

The vault encryption key is kept only in memory while the vault is unlocked and is cleared when the vault is locked.

## Tech Stack

* C#
* .NET 10
* .NET MAUI
* SQLite
* sqlite-net-pcl
* PBKDF2
* AES-GCM
* System.Security.Cryptography

## Architecture

The application is divided into several layers:

```text
┌─────────────────────────────┐
│         .NET MAUI UI        │
├─────────────────────────────┤
│         View Layer          │
├─────────────────────────────┤
│        Service Layer        │
│        VaultService         │
├─────────────────────────────┤
│       Security Layer        │
│ PBKDF2 / AES-GCM / Vault    │
├─────────────────────────────┤
│        Data Layer           │
│       DatabaseService       │
├─────────────────────────────┤
│           SQLite            │
└─────────────────────────────┘
```

The current version uses code-behind for the UI. The project is planned to be refactored to MVVM using CommunityToolkit.Mvvm.

## Database

PasswordVault uses SQLite for local storage.

The database schema was migrated from the initial plaintext model to the encrypted model without losing existing password entries.

Sensitive fields are no longer stored as individual SQLite columns. They are stored inside the encrypted `EncryptedData` blob.

## Screenshots

Screenshots will be added here.

## Platforms

The application has been tested on:

* Windows
* Android

## Project Status

The core functionality and encryption layer are implemented.

Current development focus:

* MVVM refactoring
* UI improvements
* Unit tests
* Additional security improvements
* Documentation
* Android release build

## Future Improvements

* Full MVVM architecture
* Improved notifications and toast messages
* Automatic vault locking
* Unit and integration tests
* Improved password generator UI
* UI/UX improvements
* Android release build
* Additional database and security hardening

## Disclaimer

PasswordVault is a personal portfolio and educational project. It is not intended to replace professionally audited password managers for storing highly sensitive real-world credentials.
