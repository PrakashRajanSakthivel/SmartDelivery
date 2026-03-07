-- ============================================================
-- SmartDelivery – Restaurant Seed Data
-- DB      : RestaurantServiceDb  (localdb)\MSSQLLocalDB
-- Safe    : Idempotent – run as many times as needed
-- Images  : Unsplash CDN (free, no API key required)
-- Created : 2025-08
-- ============================================================

USE [RestaurantServiceDb];
SET NOCOUNT ON;

BEGIN TRANSACTION;

-- ─────────────────────────────────────────────────────────────
-- 1. CLEAN UP E2E-INSERTED TEST RESTAURANTS
--    The E2E test creates a fresh "Testaurant" on every run.
--    Remove those so the proper seed data is visible.
-- ─────────────────────────────────────────────────────────────
DELETE mi FROM MenuItems mi
INNER JOIN Restaurants r ON mi.RestaurantId = r.Id
WHERE r.Name = 'Testaurant';

DELETE c FROM Categories c
INNER JOIN Restaurants r ON c.RestaurantId = r.Id
WHERE r.Name = 'Testaurant';

DELETE FROM Restaurants WHERE Name = 'Testaurant';

-- ─────────────────────────────────────────────────────────────
-- 2. RESTAURANTS
-- ─────────────────────────────────────────────────────────────

-- Burger House
IF NOT EXISTS (SELECT 1 FROM Restaurants WHERE Id = '11111111-0000-0000-0000-000000000001')
INSERT INTO Restaurants (Id, Name, Description, IsActive, CreatedAt,
    CoverImageUrl, LogoUrl, Address,
    DeliveryFee, MinOrderAmount, AverageRating, EstimatedDeliveryTime, PhoneNumber)
VALUES (
    '11111111-0000-0000-0000-000000000001',
    'Burger House', 'Juicy burgers and crispy fries', 1, GETUTCDATE(),
    'https://images.unsplash.com/photo-1568901346375-845664e10f5e?w=800&h=400&fit=crop&auto=format',
    'https://images.unsplash.com/photo-1550547660-d9054522f09e?w=200&h=200&fit=crop&auto=format',
    '10 Burger Lane, Sydney',
    1.99, 8.00, 4.3, 20, '555-100-0001'
);

-- Pizza Palace
IF NOT EXISTS (SELECT 1 FROM Restaurants WHERE Id = '11111111-0000-0000-0000-000000000002')
INSERT INTO Restaurants (Id, Name, Description, IsActive, CreatedAt,
    CoverImageUrl, LogoUrl, Address,
    DeliveryFee, MinOrderAmount, AverageRating, EstimatedDeliveryTime, PhoneNumber)
VALUES (
    '11111111-0000-0000-0000-000000000002',
    'Pizza Palace', 'Wood-fired pizzas made fresh daily', 1, GETUTCDATE(),
    'https://images.unsplash.com/photo-1565299624946-b28f40a0ca4b?w=800&h=400&fit=crop&auto=format',
    'https://images.unsplash.com/photo-1604382354764-a39a5b7f4b7c?w=200&h=200&fit=crop&auto=format',
    '22 Pizza Street, Melbourne',
    2.49, 12.00, 4.6, 30, '555-100-0002'
);

-- Sushi Garden
IF NOT EXISTS (SELECT 1 FROM Restaurants WHERE Id = '11111111-0000-0000-0000-000000000003')
INSERT INTO Restaurants (Id, Name, Description, IsActive, CreatedAt,
    CoverImageUrl, LogoUrl, Address,
    DeliveryFee, MinOrderAmount, AverageRating, EstimatedDeliveryTime, PhoneNumber)
VALUES (
    '11111111-0000-0000-0000-000000000003',
    'Sushi Garden', 'Authentic Japanese sushi and sashimi', 1, GETUTCDATE(),
    'https://images.unsplash.com/photo-1553621042-f6e147245754?w=800&h=400&fit=crop&auto=format',
    'https://images.unsplash.com/photo-1519984388953-d2406bc725e1?w=200&h=200&fit=crop&auto=format',
    '7 Garden Road, Brisbane',
    3.49, 15.00, 4.8, 35, '555-100-0003'
);

-- Spice of India
IF NOT EXISTS (SELECT 1 FROM Restaurants WHERE Id = '11111111-0000-0000-0000-000000000004')
INSERT INTO Restaurants (Id, Name, Description, IsActive, CreatedAt,
    CoverImageUrl, LogoUrl, Address,
    DeliveryFee, MinOrderAmount, AverageRating, EstimatedDeliveryTime, PhoneNumber)
VALUES (
    '11111111-0000-0000-0000-000000000004',
    'Spice of India', 'Authentic North Indian curries and tandoor', 1, GETUTCDATE(),
    'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=800&h=400&fit=crop&auto=format',
    'https://images.unsplash.com/photo-1585937421612-70a538812e26?w=200&h=200&fit=crop&auto=format',
    '55 Curry Lane, Perth',
    1.99, 10.00, 4.5, 40, '555-100-0004'
);

-- ─────────────────────────────────────────────────────────────
-- 3. CATEGORIES
-- ─────────────────────────────────────────────────────────────

-- Burger House
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22221111-0000-0000-0000-000000000001')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22221111-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000001', 'Burgers', 1);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22221111-0000-0000-0000-000000000002')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22221111-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000001', 'Sides',   2);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22221111-0000-0000-0000-000000000003')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22221111-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000001', 'Drinks',  3);

-- Pizza Palace
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22222222-0000-0000-0000-000000000001')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22222222-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000002', 'Pizzas',  1);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22222222-0000-0000-0000-000000000002')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22222222-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000002', 'Pastas',  2);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22222222-0000-0000-0000-000000000003')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22222222-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000002', 'Drinks',  3);

-- Sushi Garden
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22223333-0000-0000-0000-000000000001')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22223333-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000003', 'Rolls',   1);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22223333-0000-0000-0000-000000000002')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22223333-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000003', 'Sashimi', 2);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22223333-0000-0000-0000-000000000003')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22223333-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000003', 'Drinks',  3);

-- Spice of India
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22224444-0000-0000-0000-000000000001')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22224444-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000004', 'Mains',    1);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22224444-0000-0000-0000-000000000002')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22224444-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000004', 'Breads',   2);
IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = '22224444-0000-0000-0000-000000000003')
    INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder) VALUES ('22224444-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000004', 'Desserts', 3);

-- ─────────────────────────────────────────────────────────────
-- 4. MENU ITEMS
-- Columns: Id, RestaurantId, CategoryId, Name, Description,
--          Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime
-- ─────────────────────────────────────────────────────────────

-- ── Burger House → Burgers ───────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000001')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000001',
 'Classic Cheeseburger', 'Beef patty, cheese, lettuce, tomato, onion', 9.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1568901346375-845664e10f5e?w=400&h=300&fit=crop&auto=format', 10);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000002')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000001',
 'Veggie Burger', 'Plant-based patty, cheese, lettuce, tomato', 10.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1520072959219-c18dad7e2dce?w=400&h=300&fit=crop&auto=format', 10);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000003')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000001',
 'Bacon Double', 'Double beef, bacon, cheddar, pickles', 12.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1550547660-d9054522f09e?w=400&h=300&fit=crop&auto=format', 12);

-- ── Burger House → Sides ─────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000004')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000004', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000002',
 'Crispy Fries', 'Golden salted fries', 3.49, 1, 1, 1,
 'https://images.unsplash.com/photo-1573080496219-bb964010e2e7?w=400&h=300&fit=crop&auto=format', 7);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000005')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000005', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000002',
 'Onion Rings', 'Beer-battered onion rings', 4.49, 1, 1, 0,
 'https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=400&h=300&fit=crop&auto=format', 8);

-- ── Burger House → Drinks ────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000006')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000006', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000003',
 'Cola', 'Chilled Coca-Cola 500 ml', 2.49, 1, 1, 1,
 'https://images.unsplash.com/photo-1560508180-ec7b32ae70e2?w=400&h=300&fit=crop&auto=format', 2);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33331111-0000-0000-0000-000000000007')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33331111-0000-0000-0000-000000000007', '11111111-0000-0000-0000-000000000001', '22221111-0000-0000-0000-000000000003',
 'Milkshake', 'Vanilla, chocolate or strawberry', 4.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1577805947697-89e18249d767?w=400&h=300&fit=crop&auto=format', 5);

-- ── Pizza Palace → Pizzas ────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000001')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000001',
 'Margherita', 'Tomato, mozzarella, fresh basil', 11.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1604382354764-a39a5b7f4b7c?w=400&h=300&fit=crop&auto=format', 15);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000002')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000001',
 'Pepperoni', 'Tomato, mozzarella, pepperoni', 13.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1565299624946-b28f40a0ca4b?w=400&h=300&fit=crop&auto=format', 15);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000003')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000001',
 'BBQ Chicken', 'BBQ base, chicken, red onion, coriander', 14.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1571407970349-c954d5c3db1d?w=400&h=300&fit=crop&auto=format', 18);

-- ── Pizza Palace → Pastas ────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000004')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000004', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000002',
 'Spaghetti Bolognese', 'Slow-cooked beef ragù, parmesan', 12.49, 1, 0, 0,
 'https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=400&h=300&fit=crop&auto=format', 20);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000005')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000005', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000002',
 'Penne Arrabbiata', 'Spicy tomato, garlic, basil', 10.99, 1, 1, 1,
 'https://images.unsplash.com/photo-1548943487-a2e4e43b4853?w=400&h=300&fit=crop&auto=format', 18);

-- ── Pizza Palace → Drinks ────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33332222-0000-0000-0000-000000000006')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33332222-0000-0000-0000-000000000006', '11111111-0000-0000-0000-000000000002', '22222222-0000-0000-0000-000000000003',
 'San Pellegrino', 'Sparkling mineral water 330 ml', 2.99, 1, 1, 1,
 'https://images.unsplash.com/photo-1548839140-29a749e1cf4d?w=400&h=300&fit=crop&auto=format', 1);

-- ── Sushi Garden → Rolls ─────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33333333-0000-0000-0000-000000000001')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33333333-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000003', '22223333-0000-0000-0000-000000000001',
 'California Roll', 'Crab, avocado, cucumber, sesame', 12.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1553621042-f6e147245754?w=400&h=300&fit=crop&auto=format', 15);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33333333-0000-0000-0000-000000000002')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33333333-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000003', '22223333-0000-0000-0000-000000000001',
 'Spicy Tuna Roll', 'Fresh tuna, spicy mayo, cucumber', 14.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1582450921479-ddc7d07c5e76?w=400&h=300&fit=crop&auto=format', 15);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33333333-0000-0000-0000-000000000003')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33333333-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000003', '22223333-0000-0000-0000-000000000001',
 'Dragon Roll', 'Prawn tempura, avocado, eel sauce', 16.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1617196034183-421b4040ed20?w=400&h=300&fit=crop&auto=format', 18);

-- ── Sushi Garden → Sashimi ───────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33333333-0000-0000-0000-000000000004')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33333333-0000-0000-0000-000000000004', '11111111-0000-0000-0000-000000000003', '22223333-0000-0000-0000-000000000002',
 'Salmon Sashimi (6 pcs)', 'Premium Atlantic salmon, pickled ginger, wasabi', 17.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1519984388953-d2406bc725e1?w=400&h=300&fit=crop&auto=format', 10);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33333333-0000-0000-0000-000000000005')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33333333-0000-0000-0000-000000000005', '11111111-0000-0000-0000-000000000003', '22223333-0000-0000-0000-000000000003',
 'Miso Soup', 'Tofu, wakame seaweed, spring onion', 3.99, 1, 1, 1,
 'https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400&h=300&fit=crop&auto=format', 5);

-- ── Spice of India → Mains ───────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000001')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000001', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000001',
 'Chicken Tikka Masala', 'Chargrilled chicken in spiced tomato-cream sauce', 15.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400&h=300&fit=crop&auto=format', 20);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000002')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000002', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000001',
 'Lamb Rogan Josh', 'Slow-braised lamb, Kashmiri spices', 17.99, 1, 0, 0,
 'https://images.unsplash.com/photo-1585937421612-70a538812e26?w=400&h=300&fit=crop&auto=format', 25);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000003')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000003', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000001',
 'Palak Paneer', 'Cottage cheese, creamed spinach, garam masala', 13.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1574484284002-952d92456975?w=400&h=300&fit=crop&auto=format', 20);

-- ── Spice of India → Breads ──────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000004')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000004', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000002',
 'Garlic Naan', 'Tandoor-baked flatbread, garlic butter', 3.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=400&h=300&fit=crop&auto=format', 8);

IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000005')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000005', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000002',
 'Paratha', 'Whole-wheat layered bread, served with raita', 3.49, 1, 1, 0,
 'https://images.unsplash.com/photo-1565557623262-b51c2513a641?w=400&h=300&fit=crop&auto=format', 8);

-- ── Spice of India → Desserts ────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = '33334444-0000-0000-0000-000000000006')
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, IsVegan, ImageUrl, PreparationTime) VALUES
('33334444-0000-0000-0000-000000000006', '11111111-0000-0000-0000-000000000004', '22224444-0000-0000-0000-000000000003',
 'Gulab Jamun', 'Milk dumplings in rose-cardamom syrup (3 pcs)', 5.99, 1, 1, 0,
 'https://images.unsplash.com/photo-1601050690597-df0568f70950?w=400&h=300&fit=crop&auto=format', 5);

COMMIT TRANSACTION;

-- ─────────────────────────────────────────────────────────────
-- VERIFY
-- ─────────────────────────────────────────────────────────────
SELECT r.Name AS Restaurant, COUNT(DISTINCT c.Id) AS Categories, COUNT(DISTINCT m.Id) AS MenuItems
FROM Restaurants r
LEFT JOIN Categories c ON c.RestaurantId = r.Id
LEFT JOIN MenuItems  m ON m.RestaurantId = r.Id
GROUP BY r.Name
ORDER BY r.Name;
