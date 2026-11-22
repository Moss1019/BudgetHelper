//

import Foundation

public class ReceiptClient {
    let client = ApiClient()
    
    public init(){}
    
    public func getAll() async -> OpResult<[Receipt]> {
        await client.get([Receipt].self, remoteUrl: "api/receipts")
    }
    
    public func get(_ id: String) async -> OpResult<Receipt> {
        await client.get(Receipt.self, remoteUrl: "api/receipts/\(id)")
    }
    
    public func create(_ receipt: Receipt) async -> OpResult<Receipt> {
        await client.post(Receipt.self, remoteUrl: "api/receipts", value: receipt)
    }
}
