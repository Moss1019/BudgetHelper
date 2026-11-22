import SwiftUI

public class ReceiptLineService {
    let client = ReceiptLineClient()
    
    public init(){}
    
    public func getForReceipt(_ receiptId: String) async -> OpResult<[ReceiptLine]> {
        await client.getForReceipt(receiptId)
    }
    
    public func get(_ receiptLineId: String) async -> OpResult<ReceiptLine> {
        await client.get(receiptLineId)
    }
    
    public func create(_ receiptLine: ReceiptLine) async -> OpResult<ReceiptLine> {
        await client.create(receiptLine)
    }
}
