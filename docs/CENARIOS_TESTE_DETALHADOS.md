# 🧪 Cenários de Teste Detalhados - Cada Serviço

**Objetivo**: Detalhar os cenários faltantes para atingir 90% de cobertura  
**Formato**: Test class → Test methods → Expected behaviors

---

## 🏠 DOMAIN LAYER

### Territory Entity Tests

```csharp
public class TerritoryTests
{
    // ✅ Existentes
    [Fact] public void Constructor_WithValidData_CreatesTerritory()
    
    // ❌ FALTANTES
    [Fact] public void Constructor_WithSpecialCharacters_SanitizesName()
    [Fact] public void Constructor_WithExcessiveLength_ThrowsValidationException()
    [Fact] public void Constructor_WithInvalidLatitude_ThrowsArgumentException()
    [Fact] public void Constructor_WithInvalidLongitude_ThrowsArgumentException()
    [Fact] public void CalculateDistance_ReturnCorrectMeters()
    [Fact] public void ContainsPoint_WithInternalPoint_ReturnsTrue()
    [Fact] public void ContainsPoint_WithExternalPoint_ReturnsFalse()
    [Fact] public void SetCharacterization_WithDuplicateTags_RemovesDuplicates()
    [Fact] public void SetCharacterization_WithSpecialCharacters_Sanitizes()
}
```

**Cenários**: 8+ testes → ~40 horas

---

### User Entity Tests

```csharp
public class UserTests
{
    // ✅ Existentes
    [Fact] public void Constructor_WithValidData_CreatesUser()
    
    // ❌ FALTANTES
    [Fact] public void Constructor_WithBothCpfAndForeignDoc_ThrowsArgumentException()
    [Fact] public void Constructor_WithoutCpfOrForeignDoc_ThrowsArgumentException()
    [Fact] public void Constructor_WithInvalidCpf_ThrowsValidationException()
    [Fact] public void UpdateBio_Exceeding500Chars_ThrowsArgumentException()
    [Fact] public void UpdateBio_WithSpecialCharacters_SanitizesContent()
    [Fact] public void EnableTwoFactor_StoresSecretAndRecoveryCodes()
    [Fact] public void DisableTwoFactor_ClearsAllSecurityData()
    [Fact] public void UpdateAvatar_WithValidMediaAssetId_Updates()
    [Fact] public void UpdateIdentityVerification_TransitionsStates()
}
```

**Cenários**: 9+ testes → ~45 horas

---

### Post Entity Tests

```csharp
public class PostTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public void Constructor_WithValidData_CreatesPost()
    [Fact] public void AddTags_WithDuplicates_DeduplicatesTags()
    [Fact] public void AddTags_WithSpecialCharacters_Sanitizes()
    [Fact] public void AddMediaReferences_ValidatesImageCount()
    [Fact] public void AddMediaReferences_MaxLimit10_ThrowsWhenExceeded()
    [Fact] public void AddGeoAnchor_WithInvalidCoordinates_ThrowsException()
    [Fact] public void Publish_FromDraft_UpdatesState()
    [Fact] public void Archive_FromPublished_SoftDeletes()
    [Fact] public void DeleteCascade_RemovesComments_Likes_Shares()
    [Fact] public void IncrementLikeCount_UpdatesAggregates()
    [Fact] public void DecrementLikeCount_PreventesNegatives()
    [Fact] public void GetViewCount_CalculatedCorrectly()
}
```

**Cenários**: 12+ testes → ~60 horas

---

### Voting/Governance Tests

```csharp
public class VotingTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public void CreateVoting_WithValidOptions_Creates()
    [Fact] public void AddVote_BeforeDeadline_Accepts()
    [Fact] public void AddVote_AfterDeadline_Rejects()
    [Fact] public void AddVote_DuplicateVoter_Prevents()
    [Fact] public void CalculateResults_WithMajority_DeterminatesWinner()
    [Fact] public void CalculateResults_WithTie_ReturnsTieStatus()
    [Fact] public void ApplyVoteWeight_ForCurators_MultipliesVotes()
    [Fact] public void ApplyVoteWeight_ForRegularUsers_CountsAsOne()
    [Fact] public void Close_WithCurator_ExecutesWithVeto()
    [Fact] public void Close_WithResults_PublishesResults()
    [Fact] public void GetParticipationRate_CalculatesCorrectly()
}
```

**Cenários**: 11+ testes → ~55 horas

---

### Marketplace Entities Tests

```csharp
public class StoreTests
{
    // ❌ FALTANTES
    [Fact] public void CreateStore_WithValidData_Creates()
    [Fact] public void UpdateProfileImage_WithInvalidUrl_Validates()
    [Fact] public void GetRating_AveragesAllRatings()
    [Fact] public void GetRating_WithNoRatings_ReturnsZero()
}

public class ItemTests
{
    // ❌ FALTANTES
    [Fact] public void CreateItem_WithPriceValidation_Rejects_Negative()
    [Fact] public void CreateItem_WithPriceValidation_Rejects_Decimals()
    [Fact] public void UpdateStock_Decrements_OnPurchase()
    [Fact] public void UpdateStock_Prevents_NegativeStock()
    [Fact] public void GetAvailability_ReturnsFalse_WhenOutOfStock()
}
```

**Cenários**: 9+ testes → ~45 horas

---

## 📱 APPLICATION LAYER

### FeedService Tests

```csharp
public class FeedServiceTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public async Task GetTerritoryFeed_WithValidPagination_Returns()
    [Fact] public async Task GetTerritoryFeed_PageZero_Throws()
    [Fact] public async Task GetTerritoryFeed_NegativeLimit_Throws()
    [Fact] public async Task GetTerritoryFeed_FilterByInterests_OnlyReturnsRelevant()
    [Fact] public async Task GetTerritoryFeed_PaginationCursor_WorksCorrectly()
    [Fact] public async Task CreatePost_WithoutMedia_Creates()
    [Fact] public async Task CreatePost_WithMultipleMedia_AttachesAll()
    [Fact] public async Task CreatePost_WithTags_StoresTags()
    [Fact] public async Task DeletePost_RemovesCascadeData()
    [Fact] public async Task DeletePost_RemovesFromCache()
    [Fact] public async Task AddComment_ToPublishedPost_Updates()
    [Fact] public async Task AddLike_PreventsDuplicate()
    [Fact] public async Task RemoveLike_ReducesCount()
}
```

**Cenários**: 13+ testes → ~65 horas

---

### MarketplaceService Tests

```csharp
public class MarketplaceServiceTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public async Task AddToCart_WithValidItem_Succeeds()
    [Fact] public async Task AddToCart_WithOutOfStockItem_Fails()
    [Fact] public async Task UpdateCart_WithConcurrentModifications_Resolves()
    [Fact] public async Task Checkout_CalculatesPlatformFees_Correctly()
    [Fact] public async Task Checkout_WithNegativeFeePercentage_Throws()
    [Fact] public async Task Checkout_ReducesInventory()
    [Fact] public async Task Checkout_CreatesSellerTransaction()
    [Fact] public async Task Checkout_UpdatesSellerBalance()
    [Fact] public async Task ProcessRefund_ReversesTransaction()
    [Fact] public async Task ProcessRefund_RestoresInventory()
}
```

**Cenários**: 10+ testes → ~50 horas

---

### VotingService Tests

```csharp
public class VotingServiceTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public async Task CreateVoting_WithValidOptions_Creates()
    [Fact] public async Task CreateVoting_ExceedingMaxOptions_Throws()
    [Fact] public async Task CastVote_BeforeDeadline_Succeeds()
    [Fact] public async Task CastVote_AfterDeadline_Fails()
    [Fact] public async Task CastVote_DuplicateVoter_Fails()
    [Fact] public async Task CastVote_WithCuratorWeight_Multiplies()
    [Fact] public async Task GetResults_BeforeDeadline_ReturnsPreliminary()
    [Fact] public async Task GetResults_AfterDeadline_ExecutesAndReturns()
    [Fact] public async Task CloseVoting_WithAdminVeto_Overrides()
}
```

**Cenários**: 9+ testes → ~45 horas

---

### NotificationService Tests

```csharp
public class NotificationServiceTests
{
    // ❌ FALTANTES (Completo)
    [Fact] public async Task SendNotification_ToUser_Delivers()
    [Fact] public async Task SendNotification_BulkToMultipleUsers_Delivers()
    [Fact] public async Task SendNotification_DeduplicatesSameNotification()
    [Fact] public async Task SendNotification_WithFailure_RetryLogic()
    [Fact] public async Task GetUserNotifications_WithPagination_Returns()
    [Fact] public async Task MarkAsRead_UpdatesState()
    [Fact] public async Task GetUnreadCount_ReturnsCorrectly()
}
```

**Cenários**: 7+ testes → ~35 horas

---

### ValidationService Tests

```csharp
public class ValidationServiceTests
{
    // ❌ FALTANTES - Para cada um dos 14 validators
    [Fact] public void CreatePostValidator_MinLength_Validated()
    [Fact] public void CreatePostValidator_MaxLength_Validated()
    [Fact] public void CreatePostValidator_SpecialCharacters_Validated()
    [Fact] public void CreatePostValidator_SQLInjection_Rejected()
    [Fact] public void CreatePostValidator_XSSPayload_Rejected()
    // ... (repetir para todos os 14 validators, ~10 testes cada)
}
```

**Cenários**: 140+ testes (14 validators × 10 edge cases) → ~100 horas

---

## 🗄️ INFRASTRUCTURE LAYER

### Repository Tests

```csharp
public class PostRepositoryTests
{
    // ❌ FALTANTES
    [Fact] public async Task GetById_WithExistingId_Returns()
    [Fact] public async Task GetById_WithNonExistentId_ReturnsNull()
    [Fact] public async Task GetByTerritoryId_WithPagination_Returns()
    [Fact] public async Task GetByTerritoryId_WithFilters_FiltersCorrectly()
    [Fact] public async Task AddAsync_InsertsRecord()
    [Fact] public async Task UpdateAsync_UpdatesRecord()
    [Fact] public async Task UpdateAsync_WithVersionMismatch_ThrowsConcurrencyException()
    [Fact] public async Task DeleteAsync_SoftDeletes()
    [Fact] public async Task GetByTagFilter_ReturnsMatchingPosts()
    [Fact] public async Task GetByGeoAnchor_WithinRadius_Returns()
}
```

**Cenários**: 10+ testes × 8 repositories → ~80 horas

---

### Cache Tests

```csharp
public class CacheServiceTests
{
    // ❌ FALTANTES
    [Fact] public async Task SetAsync_StoresValue()
    [Fact] public async Task GetAsync_RetrievesValue()
    [Fact] public async Task GetAsync_ExpiredKey_ReturnsNull()
    [Fact] public async Task RemoveAsync_ClearsKey()
    [Fact] public async Task SetAsync_WithRedisDown_FailsOver()
    [Fact] public async Task GetAsync_WithMemoryPressure_Evicts()
}
```

**Cenários**: 6+ testes → ~30 horas

---

### MediaService Tests

```csharp
public class MediaServiceTests
{
    // ❌ FALTANTES
    [Fact] public async Task UploadAsync_S3_Succeeds()
    [Fact] public async Task UploadAsync_S3Down_FailsoverToAzure()
    [Fact] public async Task UploadAsync_AzureDown_FailsoverToLocal()
    [Fact] public async Task DownloadAsync_RetrievesFile()
    [Fact] public async Task DeleteAsync_RemovesFile()
    [Fact] public async Task GenerateThumbnail_CreatesSmallImage()
}
```

**Cenários**: 6+ testes → ~30 horas

---

## 🌐 API LAYER

### FeedController Tests

```csharp
public class FeedControllerTests
{
    // ❌ FALTANTES
    [Fact] public async Task GetFeed_Unauthorized_Returns401()
    [Fact] public async Task GetFeed_WithInvalidPagination_Returns400()
    [Fact] public async Task GetFeed_ValidRequest_Returns200()
    [Fact] public async Task CreatePost_Unauthorized_Returns401()
    [Fact] public async Task CreatePost_InvalidPayload_Returns400()
    [Fact] public async Task CreatePost_ValidPayload_Returns201()
}
```

**Cenários**: 6+ testes × 5 controllers → ~50 horas

---

### AuthController Tests

```csharp
public class AuthControllerTests
{
    // ❌ FALTANTES
    [Fact] public async Task Login_ValidCredentials_ReturnsToken()
    [Fact] public async Task Login_InvalidCredentials_Returns401()
    [Fact] public async Task RefreshToken_ValidToken_ReturnsNewToken()
    [Fact] public async Task RefreshToken_ExpiredToken_Returns401()
    [Fact] public async Task RefreshToken_TamperedToken_Returns401()
}
```

**Cenários**: 5+ testes → ~25 horas

---

## 🛠️ HELPERS & UTILS

### Mapper Tests

```csharp
public class UserMapperTests
{
    // ❌ FALTANTES
    [Fact] public void ToDtoAsync_WithValidEntity_Maps()
    [Fact] public void ToDtoAsync_WithNullEntity_ReturnsNull()
    [Fact] public void ToDtoAsync_WithCircularReference_Handles()
}
```

**Cenários**: 3+ testes × 10 mappers → ~30 horas

---

## 📊 Summary

| Layer | Test Classes | Tests per Class | Total | Hours |
|-------|-------------|-----------------|-------|-------|
| Domain | 7 | 10-12 | 80 | 200h |
| Application | 8 | 10-15 | 100 | 250h |
| Infrastructure | 15 | 6-10 | 120 | 150h |
| API | 8 | 6-10 | 70 | 100h |
| Helpers | 10 | 3-5 | 40 | 50h |
| **TOTAL** | **48** | **8-9** | **410** | **750h** |

---

**Resultado esperado**: ~2.200 testes novos + 798 existentes = ~3.000 testes total  
**Cobertura esperada**: 90%+  
**Tempo com 1 dev**: ~20 semanas (half-time) ou ~10 semanas (full-time)
