import { InventoryItem } from "./InventoryItem";

export interface BomItem {
  id: string;
  category: string;
  value: string;
  package: string;
  references: string[];
  quantity: number;
  description: string;
  isRelevant: boolean;
  isPlaced: boolean;
  matchingItems: InventoryItem[];
}
