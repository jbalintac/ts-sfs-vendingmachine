import { Component } from '@angular/core';
import { VendingMachineService } from '../core/services/vending-machine.service';
import { Coin } from '../core/models/coin.model';
import { Product } from '../core/models/product.model';
import { Observable } from 'rxjs/Observable';
import { Session } from '../core/models/session.model';
import { of } from 'rxjs/observable/of';
import { ProceduralRenderer3 } from '@angular/core/src/render3/renderer';

@Component({
  selector: 'app-vending-machine',
  templateUrl: './vending-machine.component.html',
})
export class VendingMachineComponent {

  session$: Observable<Session>;
  message = "";
  coinStatusMessage = "";


  constructor(private _vendingMachineService: VendingMachineService) {
    this.session$ = _vendingMachineService.getSession();
  }

  insertCoin(coin: Coin) {
    this.session$ = this._vendingMachineService.insertCoin(coin);
    this.message = "";
    this.coinStatusMessage = "";
  }

  returnCoins() {
    this._vendingMachineService.returnCoins().subscribe(d => {

      this.session$ = of(d.session);
      this.message = d.message;
      this.coinStatusMessage = d.coinStatusMessage;
    });
  }

  purchase(product: Product) {
    this._vendingMachineService.purchase(product).subscribe(d => {

      this.session$ = of(d.session);
      this.message = d.message;
      this.coinStatusMessage = d.coinStatusMessage;
    });
  }
}
