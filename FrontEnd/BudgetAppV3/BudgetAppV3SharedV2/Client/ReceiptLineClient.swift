//

public class ReceiptLineClient {
    let client = ApiClient()
    
    public init(){}
    
    public func getForReceipt(_ receiptId: String) async -> OpResult<[ReceiptLine]> {
        await client.get([ReceiptLine].self, remoteUrl: "api/receipts/lines/\(receiptId)")
    }
    
    public func get(_ receiptLineId: String) async -> OpResult<ReceiptLine> {
        await client.get(ReceiptLine.self, remoteUrl: "api/receipt-lines/\(receiptLineId)")
    }
    
    public func create(_ receiptLine: ReceiptLine) async -> OpResult<ReceiptLine> {
        await client.post(ReceiptLine.self, remoteUrl: "api/receipt-lines", value: receiptLine)
    }
}
