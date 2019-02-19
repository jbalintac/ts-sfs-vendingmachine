# Vending Machine

## Overview
A very simple vending machine app. 

## Requirement Checklist
| |Requirement | Notes |
|--|--|--|
| [x] | .Net core | Version: 2.1 | 
| [x] | Frontend Framework | Angular 5+  |
| [x] | Unit test | CoinChangerService unit test |

## Improvement Recommendations

### Code
- Create unit tests for the VendingMachineService, Angular components, & e2e.

### Frontend
- Add Masonry for the card display.
- Responsive and mobile friendly UI.
- The template is currently static, utilize \*ngFor
- Format decimal to ##.##

### Backend
- Add fluent validation on API
- Use constants for strings messages

## Additional Case Requirments
- If product is no longer available (0 portions), display "Product not available.". 
- If after purchasing a product and coins are not able to make a change, display "Insufficient coin to make a change.".
- If returning / cancel the inserted coins, return the fewest coin possible too.


## Build & Development server
Open in `VS 2017` to run or execute `dotnet run` on the `VendingMachine` directory. The webserver will point to [localhost:5001](https://localhost:5001).

## Running unit tests
Run `dotnet test` to execute the unit tests on `VendingMachine.Test` directory.
