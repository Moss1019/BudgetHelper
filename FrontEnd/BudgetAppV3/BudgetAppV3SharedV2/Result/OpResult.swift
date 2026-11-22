import Foundation

public struct OpResult<T> {
    public let isSuccessful: Bool
    public let content: T?
    public let message: String?
    
    private init(_ isSuccessful: Bool, _ content: T?, _ message: String?) {
        self.isSuccessful = isSuccessful
        self.content = content
        self.message = message
    }
    
    public static func success(content: T) -> OpResult<T> {
        return OpResult(true, content, nil)
    }
    
    public static func failed(message: String) -> OpResult<T> {
        return OpResult(false, nil, message)
    }
}
