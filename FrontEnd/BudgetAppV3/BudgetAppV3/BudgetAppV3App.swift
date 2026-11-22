import SwiftUI
import BudgetAppV3SharedV2

@main
struct BudgetAppV3App: App {
    @StateObject private var receiptController: ReceiptController
    @StateObject private var receiptLineController: ReceiptLineController
    @StateObject private var receiptDetectorController: ReceiptDetectorController
    
    init() {
        let budgetService = BudgetService()
        let receiptService = ReceiptService()
        let receiptLineService = ReceiptLineService()
        let receiptDetectorService = ReceiptDetectorService()
        
        let receiptController = ReceiptController(budgetService: budgetService, receiptService: receiptService)
        _receiptController = StateObject(wrappedValue: receiptController)
        
        let receiptLineController = ReceiptLineController(receiptLineService: receiptLineService)
        _receiptLineController = StateObject(wrappedValue: receiptLineController)
        
        let receiptDetectorController = ReceiptDetectorController(service: receiptDetectorService)
        _receiptDetectorController = StateObject(wrappedValue: receiptDetectorController)
    }
    
    var body: some Scene {
        WindowGroup {
//            ReceiptView()
            ReceiptDetectorView()
                .environmentObject(receiptController)
                .environmentObject(receiptLineController)
                .environmentObject(receiptDetectorController)
        }
    }
}
