import { Product } from "./product.model";
import { Coin } from "./coin.model";

export class Session {
  currentInsertedCoin: number;
  availableProducts: Array<Product>;
  availableCoins: Array<Coin>;
}
