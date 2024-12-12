export interface InventoryItem {
    id: number;
    type: string; // e.g., "SMD resistors"
    symbol: string; // Unique identifier for the item
    category: string; // Category of the item
    value: string; // e.g., "124kΩ"
    package: string; // Physical package type, e.g., "0402"
    quantity: number; // Available stock
    location: string; // Storage location
    datasheetLink: string; // URL to the item's datasheet
    storeLink: string; // URL to the store's item page
    photoUrl: string; // URL to the item's image
    minStockLevel: number; // Minimum stock level for alerts
    description: string; // Detailed description of the item
    isActive: boolean; // Indicates if the item is currently active
    tags: string[]; // Array of tags for categorization
    dateAdded: string; // ISO date format, e.g., "2024-12-06T17:25:56.229Z"
    lastUpdated: string; // ISO date format for the last update
  }