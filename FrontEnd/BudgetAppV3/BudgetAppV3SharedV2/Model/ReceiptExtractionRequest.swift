//

public class ReceiptExtractionRequest: Encodable {
    public let base64Image: String
    
    public init(base64Image: String) {
        self.base64Image = base64Image
    }
    
    enum CodingKeys: String, CodingKey {
        case base64Image
    }
}
