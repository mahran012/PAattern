# Subtask 1 - Class Diagram

## Design Pattern: Open/Closed Principle

The solution implements the Open/Closed principle by allowing new file formats to be added without modifying existing code.

## Classes and Interfaces:

```
┌─────────────────────────┐
│    IFileParser          │
├─────────────────────────┤
│ + Parse(filePath): List │
│ + CanParse(fileName):   │
│   bool                  │
└─────────────┬───────────┘
              │
        ┌─────┴─────┐
        │           │
┌───────▼────────┐  │  ┌──────────────────┐
│ JsonFileParser │  │  │  XmlFileParser   │
├────────────────┤  │  ├──────────────────┤
│ + Parse()      │  │  │ + Parse()        │
│ + CanParse()   │  │  │ + CanParse()     │
└────────────────┘  │  └──────────────────┘
                    │
┌─────────────────────────────────────────┐
│    CatalogFileProcessor               │
├─────────────────────────────────────────┤
│ - _parsers: List<IFileParser>         │
│ - _dbContext: ApplicationDbContext    │
├─────────────────────────────────────────┤
│ + ProcessCatalogFile(filePath): void   │
│ - UpdateCatalogFromItems(items): void  │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│   CatalogItemFileModel                │
├─────────────────────────────────────────┤
│ + Name: string                        │
│ + ManufacturerId: string              │
│ + CategoryName: string                │
│ + QuantityInStock: int                │
│ + PictureUrl: string                  │
│ + Attributes: List<AttributeFileModel>│
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│   AttributeFileModel                  │
├─────────────────────────────────────────┤
│ + Color: string                       │
│ + Width: double                       │
│ + Height: double                      │
│ + Length: double                      │
│ + Weight: double                      │
│ + Price: decimal                      │
└─────────────────────────────────────────┘
```

## Flow:
1. User uploads a file through CatalogAdminController
2. CatalogFileProcessor receives the file path
3. Processor iterates through registered parsers
4. Finds appropriate parser using CanParse() method
5. Uses the parser to deserialize the file into CatalogItemFileModel objects
6. Updates the database with the parsed items

## Open/Closed Principle Benefits:
- **Open for extension**: New file formats can be added by creating new classes that implement IFileParser
- **Closed for modification**: The CatalogFileProcessor and other core logic does not need to be modified when adding new formats
- **Easy maintenance**: Each parser handles its own specific format logic
- **Scalability**: The system can easily accommodate new distributors with different file formats