# TransactionData
Software Architect Technical Assignment

## Architecture
- Domain Driven Design
  
  Just use it a little bit because there is only one entity for this domain boundary
  
- CQRS
  
  To seperate Command (writing) and Query (reading) of models
  
- Onion Architecture
  

## Persistence Framework
I've chosen to use [Marten](https://github.com/JasperFx/marten) as peristence framework instead of EF core.

## WebApp
Just a normal razor pages with custom file validation from StackOverflow and some custom error pages.

## WebAPI
Nothing special.

## Additional libraries
- [CsvReader](https://github.com/JoshClose/CsvHelper)

  A popular csv content reader and writer in .NET
  
- [LanguageExt](https://github.com/louthy/language-ext)
  
  A popular FP implementation in .NET
  
- [LinqKit](https://github.com/scottksmith95/LINQKit)
  
  Make things easier when doing Linq's Expression
  
- [MediatR](https://github.com/jbogard/MediatR)
  
  To communicate with command handler in CQRS (no direct dependencies to handler)