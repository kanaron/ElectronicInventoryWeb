export interface InventoryItem {
  id: string;
  type: string;
  symbol: string;
  category: string;
  value: string;
  package: string;
  quantity: number;
  reservedForProjects: number;
  location: string;
  datasheetLink: string;
  storeLink: string;
  photoUrl: string;
  minStockLevel: number;
  description: string;
  isActive: boolean;
  tags: string[];
  dateAdded: string;
  lastUpdated: string;
}

export type InventoryItemCreate = Omit<InventoryItem, "id">;
