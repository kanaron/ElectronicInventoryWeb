export interface TmeSuggestion {
  symbol: string;
  description: string;
  totalCost: number;
  quantityToOrder: number;
  currency: string;
  url: string;
  nextPriceBreakQuantity?: number;
  nextPriceBreakUnitPrice?: number;
}

export interface PurchaseSuggestion {
  category: string;
  bomValue: string;
  package: string;
  quantityNeeded: number;
  suggestions: TmeSuggestion[];
}
