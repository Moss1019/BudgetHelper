//

public class ReceiptDetectorService {
    private let client = ReceiptDetectorClient()
    
    public init(){}
    
    public func extract(_ req: ReceiptExtractionRequest) async -> OpResult<Receipt> {
        await client.extract(req)
    }
}
