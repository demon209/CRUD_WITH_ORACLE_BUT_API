CREATE TABLE pet (
    pet_id INT PRIMARY KEY,
    pet_name VARCHAR2(100),
    pet_type VARCHAR2(50),  -- LoÀi thú cung (Chó, Mèo, ...)
    breed VARCHAR2(50),     -- Giống (Chihuahua, Mèo Ba Tu, ...)
    age INT,                -- Tuổi
    gender VARCHAR2(10),    -- Giới tính (Nam, Nữ)
    price DECIMAL(10,2),    -- Giá bán
    stock INT               -- Số lựong trong kho
);
CREATE TABLE customer (
    customer_id INT PRIMARY KEY,
    first_name VARCHAR2(100),
    last_name VARCHAR2(100),
    phone_number VARCHAR2(15),
    email VARCHAR2(100),
    address VARCHAR2(200)
);
CREATE TABLE product (
    product_id INT PRIMARY KEY,
    product_name VARCHAR2(100),
    category VARCHAR2(50),   -- Ví dụ: thức ăn, đồ chơi, dịch vụ,...
    price DECIMAL(10,2),     -- Giá 
    stock INT               -- số lượng trong kho
);
CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    customer_id INT,                       -- Mã khách hàng
    order_date DATE,                       -- Ngày đặt hàng
    total_amount DECIMAL(10,2),            -- tổng số tiền
    FOREIGN KEY (customer_id) REFERENCES customer(customer_id)
);



INSERT INTO pet VALUES (1, 'Bobby', 'Chó', 'Poodle', 2, 'Đực', 4500000.00, 5);
INSERT INTO pet VALUES (2, 'Mimi', 'Mèo', 'Mèo Anh Lông Ngắn', 1, 'Cái', 3000000.00, 3);
INSERT INTO pet VALUES (3, 'Whiskers', 'Mèo', 'Mèo Ba Tư', 2, 'Đực', 5000000.00, 4);
INSERT INTO pet VALUES (4, 'Hopper', 'Thỏ', 'Thỏ Hà Lan', 1, 'Đực', 800000.00, 6);
INSERT INTO pet VALUES (5, 'Snowball', 'Thỏ', 'Thỏ Lông Xù', 2, 'Cái', 950000.00, 4);
INSERT INTO pet VALUES (6, 'Nibbles', 'Chuột', 'Chuột Hamster', 1, 'Cái', 150000.00, 10);
INSERT INTO pet VALUES (7, 'Sparky', 'Chuột', 'Chuột Bạch', 1, 'Đực', 120000.00, 8);
INSERT INTO pet VALUES (8, 'Froggy', 'Ếch', 'Ếch Cảnh Xanh', 1, 'Đực', 200000.00, 5);
INSERT INTO pet VALUES (9, 'Greeny', 'Ếch', 'Ếch Sông Nam Mỹ', 1, 'Cái', 250000.00, 3);
INSERT INTO pet VALUES (10, 'Shelly', 'Rùa', 'Rùa Tai Đỏ', 3, 'Cái', 400000.00, 6);
INSERT INTO pet VALUES (11, 'Speedy', 'Rùa', 'Rùa Cạn Châu Phi', 4, 'Đực', 1200000.00, 2);
INSERT INTO pet VALUES (12, 'Spike', 'Nhím', 'Nhím Cảnh Châu Phi', 2, 'Đực', 900000.00, 4);
INSERT INTO pet VALUES (13, 'Fluffy', 'Nhím', 'Nhím Cảnh Bạch Tạng', 1, 'Cái', 950000.00, 3);
INSERT INTO pet VALUES (14, 'Chippy', 'Sóc', 'Sóc Bắc Mỹ', 1, 'Đực', 1000000.00, 2);
INSERT INTO pet VALUES (15, 'Sandy', 'Sóc', 'Sóc Bay Nhật Bản', 2, 'Cái', 1200000.00, 2);
INSERT INTO pet VALUES (16, 'Kiki', 'Chó', 'Chihuahua', 3, 'Cái', 2500000.00, 4);
INSERT INTO pet VALUES (17, 'Tom', 'Mèo', 'Mèo Xiêm', 2, 'Đực', 2200000.00, 5);
INSERT INTO pet VALUES (18, 'Pumpkin', 'Thỏ', 'Thỏ Mini Rex', 1, 'Cái', 900000.00, 4);
INSERT INTO pet VALUES (19, 'Dotty', 'Chuột', 'Chuột Fancy', 1, 'Cái', 130000.00, 6);
INSERT INTO pet VALUES (20, 'Gizmo', 'Chuột', 'Hamster Bear', 1, 'Đực', 170000.00, 7);
INSERT INTO pet VALUES (21, 'Pebbles', 'Ếch', 'Ếch Xanh Châu Phi', 1, 'Cái', 230000.00, 3);
INSERT INTO pet VALUES (22, 'Rocky', 'Rùa', 'Rùa Cảnh Indo', 2, 'Đực', 500000.00, 3);
INSERT INTO pet VALUES (23, 'Pine', 'Nhím', 'Nhím Muối Tiêu', 1, 'Cái', 850000.00, 3);
INSERT INTO pet VALUES (24, 'Lilo', 'Sóc', 'Sóc Châu Á', 1, 'Đực', 1100000.00, 2);
INSERT INTO pet VALUES (25, 'Neko', 'Mèo', 'Mèo Munchkin', 1, 'Cái', 5500000.00, 3);
INSERT INTO pet VALUES (26, 'Rex', 'Chó', 'Corgi', 2, 'Đực', 7500000.00, 3);
INSERT INTO pet VALUES (27, 'Carrot', 'Thỏ', 'Thỏ Mỹ', 2, 'Cái', 980000.00, 4);
INSERT INTO pet VALUES (28, 'Twinkle', 'Sóc', 'Sóc Bay Úc', 1, 'Cái', 1250000.00, 2);
INSERT INTO pet VALUES (29, 'Taco', 'Chuột', 'Hamster Robo', 1, 'Đực', 160000.00, 6);
INSERT INTO pet VALUES (30, 'Echo', 'Ếch', 'Ếch Ăn Đỏ', 1, 'Đực', 220000.00, 4);

COMMIT;






INSERT INTO customer VALUES (1, 'Nguyen', 'An', '0905123456', 'an.nguyen@example.com', '123 Le Loi, Q1, HCM');
INSERT INTO customer VALUES (2, 'Tran', 'Binh', '0906123457', 'binh.tran@example.com', '45 Nguyen Trai, Q5, HCM');
INSERT INTO customer VALUES (3, 'Le', 'Cuong', '0907123458', 'cuong.le@example.com', '89 Hai Ba Trung, Q3, HCM');
INSERT INTO customer VALUES (4, 'Pham', 'Dung', '0908123459', 'dung.pham@example.com', '12 Cach Mang Thang 8, Q10, HCM');
INSERT INTO customer VALUES (5, 'Hoang', 'Em', '0909123460', 'em.hoang@example.com', '34 Tran Hung Dao, Q1, HCM');
INSERT INTO customer VALUES (6, 'Do', 'Phuc', '0910123461', 'phuc.do@example.com', '56 Vo Van Tan, Q3, HCM');
INSERT INTO customer VALUES (7, 'Bui', 'Giang', '0911123462', 'giang.bui@example.com', '78 Ly Thuong Kiet, Q10, HCM');
INSERT INTO customer VALUES (8, 'Dang', 'Hoa', '0912123463', 'hoa.dang@example.com', '90 Pasteur, Q1, HCM');
INSERT INTO customer VALUES (9, 'Nguyen', 'Hieu', '0913123464', 'hieu.nguyen@example.com', '101 Nguyen Dinh Chieu, Q3, HCM');
INSERT INTO customer VALUES (10, 'Tran', 'Khanh', '0914123465', 'khanh.tran@example.com', '23 Nguyen Van Cu, Q5, HCM');
INSERT INTO customer VALUES (11, 'Le', 'Lan', '0915123466', 'lan.le@example.com', '78 Dinh Tien Hoang, Q1, HCM');
INSERT INTO customer VALUES (12, 'Pham', 'Minh', '0916123467', 'minh.pham@example.com', '34 Nguyen Huu Canh, Binh Thanh');
INSERT INTO customer VALUES (13, 'Hoang', 'Nga', '0917123468', 'nga.hoang@example.com', '56 Dien Bien Phu, Q3, HCM');
INSERT INTO customer VALUES (14, 'Do', 'Oanh', '0918123469', 'oanh.do@example.com', '67 Vo Thi Sau, Q1, HCM');
INSERT INTO customer VALUES (15, 'Bui', 'Phat', '0919123470', 'phat.bui@example.com', '89 Nguyen Thi Minh Khai, Q3, HCM');
INSERT INTO customer VALUES (16, 'Dang', 'Quyen', '0920123471', 'quyen.dang@example.com', '120 Hoang Van Thu, Phu Nhuan');
INSERT INTO customer VALUES (17, 'Nguyen', 'Son', '0921123472', 'son.nguyen@example.com', '230 Le Van Sy, Tan Binh');
INSERT INTO customer VALUES (18, 'Tran', 'Tam', '0922123473', 'tam.tran@example.com', '88 Nam Ky Khoi Nghia, Q1, HCM');
INSERT INTO customer VALUES (19, 'Le', 'Uyen', '0923123474', 'uyen.le@example.com', '12 Phan Dinh Phung, Phu Nhuan');
INSERT INTO customer VALUES (20, 'Pham', 'Vy', '0924123475', 'vy.pham@example.com', '44 Truong Dinh, Q3, HCM');
INSERT INTO customer VALUES (21, 'Hoang', 'Xuan', '0925123476', 'xuan.hoang@example.com', '35 Le Duan, Q1, HCM');
INSERT INTO customer VALUES (22, 'Do', 'Yen', '0926123477', 'yen.do@example.com', '99 Nguyen Thai Hoc, Q1, HCM');
INSERT INTO customer VALUES (23, 'Bui', 'Anh', '0927123478', 'anh.bui@example.com', '23 Pham Viet Chanh, Binh Thanh');
INSERT INTO customer VALUES (24, 'Dang', 'Bach', '0928123479', 'bach.dang@example.com', '14 Phan Xich Long, Phu Nhuan');
INSERT INTO customer VALUES (25, 'Nguyen', 'Chi', '0929123480', 'chi.nguyen@example.com', '77 Cach Mang Thang 8, Q10');
INSERT INTO customer VALUES (26, 'Tran', 'Dao', '0930123481', 'dao.tran@example.com', '112 Ly Tu Trong, Q1, HCM');
INSERT INTO customer VALUES (27, 'Le', 'Giang', '0931123482', 'giang.le@example.com', '56 Pham Ngu Lao, Q1, HCM');
INSERT INTO customer VALUES (28, 'Pham', 'Hanh', '0932123483', 'hanh.pham@example.com', '78 Nguyen Van Troi, Phu Nhuan');
INSERT INTO customer VALUES (29, 'Hoang', 'Kien', '0933123484', 'kien.hoang@example.com', '89 Hoang Hoa Tham, Binh Thanh');
INSERT INTO customer VALUES (30, 'Do', 'Linh', '0934123485', 'linh.do@example.com', '101 Tan Son Nhi, Tan Phu');
commit;




INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (2, 2, TO_DATE('2025-04-24', 'YYYY-MM-DD'), 1200.50);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (3, 3, TO_DATE('2025-04-23', 'YYYY-MM-DD'), 1500.75);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (4, 4, TO_DATE('2025-04-22', 'YYYY-MM-DD'), 950.25);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (5, 5, TO_DATE('2025-04-21', 'YYYY-MM-DD'), 1100.00);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (6, 6, TO_DATE('2025-04-20', 'YYYY-MM-DD'), 2000.40);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (7, 7, TO_DATE('2025-04-19', 'YYYY-MM-DD'), 1450.30);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (8, 8, TO_DATE('2025-04-18', 'YYYY-MM-DD'), 1325.60);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (9, 9, TO_DATE('2025-04-17', 'YYYY-MM-DD'), 890.75);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (10, 10, TO_DATE('2025-04-16', 'YYYY-MM-DD'), 1675.80);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (11, 11, TO_DATE('2025-04-15', 'YYYY-MM-DD'), 2005.25);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (12, 12, TO_DATE('2025-04-14', 'YYYY-MM-DD'), 980.00);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (13, 13, TO_DATE('2025-04-13', 'YYYY-MM-DD'), 1520.40);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (14, 14, TO_DATE('2025-04-12', 'YYYY-MM-DD'), 1420.55);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (15, 15, TO_DATE('2025-04-11', 'YYYY-MM-DD'), 1105.20);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (16, 16, TO_DATE('2025-04-10', 'YYYY-MM-DD'), 1890.00);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (17, 17, TO_DATE('2025-04-09', 'YYYY-MM-DD'), 1345.10);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (18, 18, TO_DATE('2025-04-08', 'YYYY-MM-DD'), 1250.80);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (19, 19, TO_DATE('2025-04-07', 'YYYY-MM-DD'), 1095.50);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (20, 20, TO_DATE('2025-04-06', 'YYYY-MM-DD'), 1050.00);

INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (21, 21, TO_DATE('2025-04-05', 'YYYY-MM-DD'), 1115.40);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (22, 22, TO_DATE('2025-04-04', 'YYYY-MM-DD'), 1215.60);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (23, 23, TO_DATE('2025-04-03', 'YYYY-MM-DD'), 980.20);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (24, 24, TO_DATE('2025-04-02', 'YYYY-MM-DD'), 1150.30);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (25, 25, TO_DATE('2025-04-01', 'YYYY-MM-DD'), 1260.00);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (26, 26, TO_DATE('2025-03-31', 'YYYY-MM-DD'), 1340.25);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (27, 27, TO_DATE('2025-03-30', 'YYYY-MM-DD'), 1210.90);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (28, 28, TO_DATE('2025-03-29', 'YYYY-MM-DD'), 1455.10);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (29, 29, TO_DATE('2025-03-28', 'YYYY-MM-DD'), 1090.60);
INSERT INTO orders (order_id, customer_id, order_date, total_amount) VALUES (30, 30, TO_DATE('2025-03-27', 'YYYY-MM-DD'), 1300.75);

commit;






-- SP ------------------------------------------------------------------------------------------------------
-- Thêm thú cưng
CREATE OR REPLACE PROCEDURE add_pet (
    p_pet_id IN INT,
    p_pet_name IN VARCHAR2,
    p_pet_type IN VARCHAR2,
    p_breed IN VARCHAR2,
    p_age IN INT,
    p_gender IN VARCHAR2,
    p_price IN DECIMAL,
    p_stock IN INT,
    p_message OUT VARCHAR2
) IS
BEGIN

    INSERT INTO pet (pet_id, pet_name, pet_type, breed, age, gender, price, stock)
    VALUES (p_pet_id, p_pet_name, p_pet_type, p_breed, p_age, p_gender, p_price, p_stock);
    

    p_message := 'Them thu cung thanh cong!';

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi: ' || SQLERRM;
END;
/


-- Thêm khách hàng
CREATE OR REPLACE PROCEDURE add_customer(
    p_customer_id IN INT,
    p_first_name IN VARCHAR2,
    p_last_name IN VARCHAR2,
    p_phone_number IN VARCHAR2,
    p_email IN VARCHAR2,
    p_address IN VARCHAR2,
    p_message OUT VARCHAR2  
) IS
BEGIN
    INSERT INTO customer (customer_id, first_name, last_name, phone_number, email, address)
    VALUES (p_customer_id, p_first_name, p_last_name, p_phone_number, p_email, p_address);
    COMMIT;
    p_message := 'Them khach hang thanh cong!'; 
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        p_message := 'Lỗi: Mã khách hàng đã tồn tại.'; 
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM; 
END;
/


-- Thêm sản phẩm
CREATE OR REPLACE PROCEDURE add_product(
    p_product_id    IN INT,
    p_product_name  IN VARCHAR2,
    p_category      IN VARCHAR2,
    p_price         IN DECIMAL,
    p_stock         IN INT,
    p_message       OUT VARCHAR2
) IS
BEGIN
    INSERT INTO product (product_id, product_name, category, price, stock)
    VALUES (p_product_id, p_product_name, p_category, p_price, p_stock);
    
    COMMIT;
    p_message := 'Them san pham thanh cong!';
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        p_message := 'Lỗi: Mã sản phẩm đã tồn tại.';
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


-- Thêm đơn hàng
CREATE OR REPLACE PROCEDURE add_order(
    p_order_id IN INT,
    p_customer_id IN INT,
    p_order_date IN DATE,
    p_total_amount IN DECIMAL,
    p_message OUT VARCHAR2
) IS
    v_count INT;
BEGIN

    SELECT COUNT(*) INTO v_count
    FROM customer
    WHERE customer_id = p_customer_id;
    
    IF v_count = 0 THEN
        p_message := 'Lỗi: Mã khách hàng không tồn tại.';
        RETURN;
    END IF;


    INSERT INTO orders (order_id, customer_id, order_date, total_amount)
    VALUES (p_order_id, p_customer_id, p_order_date, p_total_amount);

    COMMIT;
    p_message := 'Them hoa don thanh cong!';
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/




-- Sửa thông tin thú cưng
CREATE OR REPLACE PROCEDURE update_pet(
    p_pet_id IN INT,
    p_pet_name IN VARCHAR2,
    p_pet_type IN VARCHAR2,
    p_breed IN VARCHAR2,
    p_age IN INT,
    p_gender IN VARCHAR2,
    p_price IN DECIMAL,
    p_stock IN INT,
    p_message OUT VARCHAR2  
) IS
BEGIN

    UPDATE pet
    SET pet_name = p_pet_name,
        pet_type = p_pet_type,
        breed = p_breed,
        age = p_age,
        gender = p_gender,
        price = p_price,
        stock = p_stock
    WHERE pet_id = p_pet_id;


    IF SQL%ROWCOUNT > 0 THEN
        p_message := 'Cap nhat thong tin thu cung thanh cong!';
    ELSE
        p_message := 'Lỗi: Không tìm thấy thú cưng với mã ID ' || p_pet_id;
    END IF;

    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
        ROLLBACK; 
END;
/


-- Sửa thông tin khách hàng
CREATE OR REPLACE PROCEDURE update_customer(
    p_customer_id     IN  INT,
    p_first_name      IN  VARCHAR2,
    p_last_name       IN  VARCHAR2,
    p_phone_number    IN  VARCHAR2,
    p_email           IN  VARCHAR2,
    p_address         IN  VARCHAR2,
    p_message         OUT VARCHAR2
) IS
BEGIN

    UPDATE customer
    SET first_name   = p_first_name,
        last_name    = p_last_name,
        phone_number = p_phone_number,
        email        = p_email,
        address      = p_address
    WHERE customer_id = p_customer_id;


    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy khách hàng với mã ID ' || p_customer_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat thong tin khach hang thanh cong!';
    END IF;
    
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


-- Sửa thông tin sản phẩm
CREATE OR REPLACE PROCEDURE update_product(
    p_product_id   IN INT,
    p_product_name IN VARCHAR2,
    p_category     IN VARCHAR2,
    p_price        IN DECIMAL,
    p_stock        IN INT,
    p_message      OUT VARCHAR2
) IS
BEGIN
    UPDATE product
    SET product_name = p_product_name,
        category     = p_category,
        price        = p_price,
        stock        = p_stock
    WHERE product_id = p_product_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy sản phẩm với mã ID ' || p_product_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat thong tin san pham thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


-- SỬA ĐƠN HÀNG
CREATE OR REPLACE PROCEDURE update_order(
    p_order_id       IN INT,
    p_customer_id    IN INT,
    p_order_date     IN DATE,
    p_total_amount   IN DECIMAL,
    p_message        OUT VARCHAR2
) IS
    v_count INT;
BEGIN

    SELECT COUNT(*) INTO v_count
    FROM customer
    WHERE customer_id = p_customer_id;

    IF v_count = 0 THEN
        p_message := 'Lỗi: Mã khách hàng ' || p_customer_id || ' không tồn tại.';
        RETURN;
    END IF;


    UPDATE orders
    SET customer_id = p_customer_id,
        order_date = p_order_date,
        total_amount = p_total_amount
    WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy đơn hàng với mã ID ' || p_order_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat don hang thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/




-- Xóa thú cưng
CREATE OR REPLACE PROCEDURE delete_pet(
    p_pet_id IN INT,
    p_message OUT VARCHAR2 
) IS
BEGIN
    DELETE FROM pet WHERE pet_id = p_pet_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy thú cưng với mã ID ' || p_pet_id;
    ELSE
        COMMIT;
        p_message := 'Xoa thu cung thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


-- Xóa khách hàng
CREATE OR REPLACE PROCEDURE delete_customer(
    p_customer_id IN INT,
    p_message OUT VARCHAR2
) IS
BEGIN

    DELETE FROM orders WHERE customer_id = p_customer_id;


    DELETE FROM customer WHERE customer_id = p_customer_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy khách hàng với mã ID ' || p_customer_id;
    ELSE
        COMMIT;
        p_message := 'Xoa khach hang thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/

-- XÓA SẢN PHẨM
CREATE OR REPLACE PROCEDURE delete_product(
    p_product_id IN INT,
    p_message    OUT VARCHAR2
) IS
BEGIN
    DELETE FROM product WHERE product_id = p_product_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy sản phẩm với mã ID ' || p_product_id;
    ELSE
        COMMIT;
        p_message := 'Xoa san pham thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


-- Xóa đơn hàng
CREATE OR REPLACE PROCEDURE delete_order(
    p_order_id IN INT,
    p_message OUT VARCHAR2
) IS
BEGIN
    DELETE FROM orders WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy đơn hàng với mã ID ' || p_order_id;
    ELSE
        COMMIT;
        p_message := 'Xoa don hang thanh cong!';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/
