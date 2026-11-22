//

public class ReceiptDetectorClient {
    let client = ApiClient()
    
    public init(){}
    
    public func extract(_ req: ReceiptExtractionRequest) async -> OpResult<Receipt> {
        await client.post(Receipt.self, remoteUrl: "api/receipt-extraction", value: req)
    }
}

