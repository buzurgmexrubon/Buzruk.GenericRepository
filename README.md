<p align="center">
  <img src="https://github.com/buzurgmexrubon/Buzruk.GenericRepository/blob/master/icon.png" height="128">
  <h1 align="center">Buzruk.GenericRepository</h1>
</p>

# The Future of .NET Data Access is Here!

**Say goodbye to repetitive CRUD boilerplate and hello to a streamlined, asynchronous-first approach to data access in your .NET applications! Buzruk.GenericRepository is your supercharged generic repository, offering a unified interface to work with various entities. Focus on building amazing features while this package handles the data interaction heavy lifting.**

[![License MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Table of Contents

- [Overview](#overview)
- [Features](#features)
	- [Unleash the Power of Asynchronous Excellence](#unleash-the-power-of-asynchronous-excellence)
	- [Intuitive Data Management](#intuitive-data-management)
	- [Beyond the Basics](#beyond-the-basics)
	- [A Collaborative Future](#a-collaborative-future)
- [Get Started in a Flash!](#get-started-in-a-flash)
	- [Installation](#installation)
	- [Usage Example](#usage-example)
- [Dive Deeper!](#dive-deeper)
- [Contributing](#contributing)
- [License](#license)
- [Changelogs](#changelogs)

## Overview

This generic repository simplifies data access in .NET applications by offering a unified interface for working with various entity types. It supports both synchronous and asynchronous methods, providing you with flexibility based on your application's requirements. By using this repository, you can focus on business logic and reduce boilerplate code related to data interactions.

## Features

### Unleash the Power of Asynchronous Excellence

- **Asynchronous by Default**: Embrace the power of asynchronous methods for **non-blocking, efficient data retrieval and manipulation. Free your UI threads from unnecessary waits!** ⚡️
- **Synchronous Options**: Maintain compatibility with legacy code or specific use cases with the included synchronous counterparts. **Versatility at your fingertips!**
- **Effortless Bulk Operations (Asynchronous)**: Optimize performance with AddRangeAsync and UpdateRangeAsync to add or update multiple entities efficiently. **Save processing power for what matters.** ⚡️

### Intuitive Data Management

- **Essential CRUD Operations**: Perform Create, Read, Update, and Delete (CRUD) actions with ease using methods like `GetAsync`, `GetPagedAsync`, `AddAsync`, `AddRangeAsync`, `UpdateAsync`, `UpdateRangeAsync`, `RemoveAsync`, and `RemoveRangeAsync`. **Simplify complex data management.**
- **Flexible Retrieval**: Find specific entities by ID or filter them based on your criteria using the versatile `GetAsync` method. **Get exactly the data you need, when you need it.**
- **Paged Data Fetching (Asynchronous)**: Handle large datasets gracefully with `GetPagedAsync`. Retrieve data in manageable chunks, ideal for pagination and performance optimization. **No more overwhelming data loads!** ⚖️
- **Existence Checks**: Quickly confirm entity existence with `ExistsAsync` to make informed decisions in your code. **Avoid unnecessary database calls.**
- **Efficient Counting**: Get the total number of entities (`CountAsync`) or utilize `LongCountAsync` for accurate counts even with very large datasets. **Always have a clear picture of your data.**
- **Targeted Counting (Asynchronous)**: Count entities based on specific conditions with `CountByAsync`. **Gain deeper insights into your data distribution.**
- **Effortless Integration with Entity Framework Core**: Designed to work seamlessly with Entity Framework Core, allowing you to leverage its powerful features. **No need to reinvent the wheel!**

### Beyond the Basics

- **Eager Loading (Optional)**: Boost performance by pre-fetching related entities when retrieving primary data (asynchronous option available: `EagerLoadAsync`), minimizing subsequent database calls. **Reduce roundtrips and improve responsiveness.**
- **Change Tracking (Optional)**: Track entity modifications for efficient saving updates (requires configuration). **Maintain data consistency with minimal effort.**

## A Collaborative Future

- **Expanding Horizons**: Community contributions are welcome to broaden support for additional data access providers beyond Entity Framework Core! **Let's make this a truly universal solution!**
- **Testing Excellence**: We're continuously striving to enhance unit testing coverage for unmatched reliability. **Confidence in your data access layer is key!** ✅
- **Empowering Documentation**: Expect even more detailed examples, tutorials, and guides to make you a data access pro! Become a master with comprehensive learning resources.

## Get Started in a Flash!

### Installation

- Installation: Integrate the magic of this package into your project using NuGet:

```bash
dotnet add package Buzruk.GenericRepository
```

### Usage Example

- Namespace Reference: Import the namespace to access the repository's power in your code:

```csharp
using Buzruk.GenericRepository
```

- Activate Your Data Access Superpowers!:  Create an instance of the repository and unleash its methods on your data:

```csharp
// Example usage (assuming Entity Framework Core context named 'dbContext')
var repository = new GenericRepository<Product>(dbContext);

var product = await repository.GetAsync(1); // Retrieve product by ID
if (product != null)
{
    product.Name = "Updated Product Name";
    await repository.UpdateAsync(product); // Update the product
}

var products = await repository.GetPagedAsync(pageNumber: 2, pageSize: 10); // Retrieve products in a paged format

// ... (other methods usage)
```

## Dive Deeper!

Explore our comprehensive Wiki for in-depth guidance on usage examples, configuration options, and advanced features:

[Link to GitHub Wiki](https://github.com/buzurgmexrubon/Buzruk.GenericRepository/wiki)

## Contributing

We welcome contributions to improve this repository! Here's how you can contribute:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Implement your changes and write unit tests for them.
4. Submit a well-documented pull request.

For more detailed guidelines, please refer to our CONTRIBUTING.md file:

[Link to CONTRIBUTING.md file](https://github.com/buzurgmexrubon/Buzruk.GenericRepository/blob/master/CONTRIBUTING.md)

## Changelogs

We maintain a detailed changelog of changes made to the package in the CHANGELOG.md file:

[Link to CHANGELOG.md file](https://github.com/buzurgmexrubon/Buzruk.GenericRepository/blob/master/CHANGELOG.md)