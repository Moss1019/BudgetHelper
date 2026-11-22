import SwiftUI
import Combine
import BudgetAppV3SharedV2

class ReceiptLineController: ObservableObject {
    private var receiptLineService: ReceiptLineService
    
    @Published var lines: [ReceiptLine]?
    
    @Published var isLoading = true
    @Published var errorMessage: String?
    
    init(receiptLineService: ReceiptLineService) {
        self.receiptLineService = receiptLineService
    }
    
    func reload(_ receiptId: String) async {
        isLoading = true
        errorMessage = nil
        
        let getForReceiptRes = await receiptLineService.getForReceipt(receiptId)
        
        guard !Task.isCancelled else {
            isLoading = false
            errorMessage = "Task was cancelled"
            return
        }
        
        if getForReceiptRes.isSuccessful {
            self.lines = getForReceiptRes.content ?? []
        } else {
            self.lines = []
        }
        
        isLoading = false
    }
}
