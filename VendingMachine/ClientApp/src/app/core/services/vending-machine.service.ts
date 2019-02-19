//import { Component } from '@angular/core';

//@Component({
//  selector: 'app-vending-machine',
//  templateUrl: './vending-machine.component.html',
//})
//export class VendingMachineComponent {
//}



import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs/observable';

import { Session } from "../models/session.model"
import { Coin } from '../models/coin.model';
import { Transaction } from '../models/transaction.model';
import { Product } from '../models/product.model';

@Injectable()
export class VendingMachineService {

  private _baseUrl = 'api/vendingmachine';

  constructor(private _httpClient: HttpClient) { }


  getSession(): Observable<Session> {
    return this._httpClient.get(this._baseUrl + "/session").pipe(
      map((response) => <Session>response));
  }


  insertCoin(coin: Coin): Observable<Session> {
    return this._httpClient.post(this._baseUrl + "/insertcoin", coin).pipe(
      map((response) => <Session>response));
  }

  returnCoins(): Observable<Transaction> {
    return this._httpClient.post(this._baseUrl + "/returncoins", null).pipe(
      map((response) => <Transaction>response));
  }

  purchase(product: Product): Observable<Transaction> {
    return this._httpClient.post(this._baseUrl + "/purchase", product).pipe(
      map((response) => <Transaction>response));
  }
}
