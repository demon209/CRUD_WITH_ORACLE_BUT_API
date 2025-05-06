
----------------------------------------------------------CREATE TABLE--------------------------------------------------------

-- create sequence for auto create id 
CREATE SEQUENCE seq_pet START WITH 1 INCREMENT BY 1;

-- create table PET
CREATE TABLE pet (
    pet_id INT PRIMARY KEY,
    pet_name VARCHAR2(100),
    pet_type VARCHAR2(50),
    breed VARCHAR2(50),
    age INT,
    gender VARCHAR2(10),
    price DECIMAL(10,2),
    stock INT
);

-- create trigger
CREATE OR REPLACE TRIGGER trg_pet_id
BEFORE INSERT ON pet
FOR EACH ROW
BEGIN
    :NEW.pet_id := seq_pet.NEXTVAL;
END;
/

-- TABLE CUSTOMER
CREATE SEQUENCE seq_customer START WITH 1 INCREMENT BY 1;

CREATE TABLE customer (
    customer_id INT PRIMARY KEY,
    first_name VARCHAR2(100),
    last_name VARCHAR2(100),
    phone_number VARCHAR2(15),
    email VARCHAR2(100),
    address VARCHAR2(200)
);

CREATE OR REPLACE TRIGGER trg_customer_id
BEFORE INSERT ON customer
FOR EACH ROW
BEGIN
    :NEW.customer_id := seq_customer.NEXTVAL;
END;
/


-- TABLE PRODUCT
CREATE SEQUENCE seq_product START WITH 1 INCREMENT BY 1;

CREATE TABLE product (
    product_id INT PRIMARY KEY,
    product_name VARCHAR2(100),
    category VARCHAR2(50),
    price DECIMAL(10,2),
    stock INT
);

CREATE OR REPLACE TRIGGER trg_product_id
BEFORE INSERT ON product
FOR EACH ROW
BEGIN
    :NEW.product_id := seq_product.NEXTVAL;
END;
/

-- TABLE ORDER
CREATE SEQUENCE seq_orders START WITH 1 INCREMENT BY 1;

CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    customer_id INT,
    order_date DATE,
    total_amount DECIMAL(10,2),
    FOREIGN KEY (customer_id) REFERENCES customer(customer_id)
);

CREATE OR REPLACE TRIGGER trg_orders_id
BEFORE INSERT ON orders
FOR EACH ROW
BEGIN
    :NEW.order_id := seq_orders.NEXTVAL;
END;
/

-- TABLE CUSTOMER_PET
CREATE TABLE customer_pet (
    customer_pet_id INT PRIMARY KEY,
    customer_id INT,
    pet_id INT,
    purchase_date DATE,
    price_at_purchase DECIMAL(10,2), -- PRICE
    FOREIGN KEY (customer_id) REFERENCES customer(customer_id),
    FOREIGN KEY (pet_id) REFERENCES pet(pet_id)
);
CREATE SEQUENCE seq_customer_pet START WITH 1 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER trg_customer_pet_id
BEFORE INSERT ON customer_pet
FOR EACH ROW
BEGIN
    :NEW.customer_pet_id := seq_customer_pet.NEXTVAL;
END;
/

















/*DROP TABLE orders CASCADE CONSTRAINTS;
DROP TABLE pet CASCADE CONSTRAINTS;
DROP TABLE customer CASCADE CONSTRAINTS;
DROP TABLE product CASCADE CONSTRAINTS;*/





---------------------------------------------------------------------INSERT DATA-------------------------------------------------------------



INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Bobby', 'Chó', 'Poodle', 2, 'Đực', 4500000, 5);

INSERT INTO pet (pet_name,  pet_type, breed, age, gender, price, stock) VALUES
('Mimi', 'Mèo', 'Mèo Anh Lông Ngắn', 1, 'Cái', 3000000, 3);

INSERT INTO pet (pet_name,  pet_type, breed, age, gender, price, stock) VALUES
('Whiskers', 'Mèo', 'Mèo Ba Tư', 2, 'Đực', 5000000, 4);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Rex', 'Chó', 'Golden Retriever', 3, 'Đực', 6000000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Luna', 'Mèo', 'Mèo Xiêm', 2, 'Cái', 3500000, 6);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Rocky', 'Chó', 'Bulldog', 4, 'Đực', 7000000, 3);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Coco', 'Chó', 'Chihuahua', 1, 'Cái', 3200000, 4);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Snowy', 'Mèo', 'Mèo Ragdoll', 2, 'Cái', 5500000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Max', 'Chó', 'Labrador', 3, 'Đực', 5800000, 3);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Kitty', 'Mèo', 'Mèo Munchkin', 1, 'Cái', 4000000, 5);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Benji', 'Chó', 'Pug', 2, 'Đực', 3800000, 6);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Simba', 'Mèo', 'Mèo Maine Coon', 4, 'Đực', 6200000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Daisy', 'Chó', 'Cocker Spaniel', 3, 'Cái', 4700000, 3);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Leo', 'Chó', 'Shih Tzu', 2, 'Đực', 3500000, 4);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Chloe', 'Mèo', 'Mèo Anh Lông Dài', 3, 'Cái', 5200000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Oscar', 'Chó', 'Samoyed', 1, 'Đực', 8000000, 1);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Bella', 'Mèo', 'Mèo Scottish Fold', 2, 'Cái', 4800000, 3);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Teddy', 'Chó', 'Bichon Frise', 1, 'Đực', 4200000, 4);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Lily', 'Mèo', 'Mèo Sphynx', 2, 'Cái', 7500000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Bruno', 'Chó', 'Doberman', 3, 'Đực', 9000000, 1);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Angel', 'Mèo', 'Mèo Bengal', 1, 'Cái', 6700000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Shadow', 'Chó', 'Husky', 2, 'Đực', 7500000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Ruby', 'Mèo', 'Mèo Mỹ Tai Cụp', 3, 'Cái', 5300000, 1);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Zorro', 'Chó', 'Beagle', 2, 'Đực', 3900000, 3);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Lola', 'Mèo', 'Mèo Himalaya', 2, 'Cái', 5600000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Thor', 'Chó', 'Rottweiler', 3, 'Đực', 8200000, 1);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Zoe', 'Mèo', 'Mèo Somali', 2, 'Cái', 6000000, 2);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock) VALUES
('Spike', 'Chó', 'Chó Phú Quốc', 4, 'Đực', 5000000, 2);


COMMIT;







INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Nguyen', 'An', '0905123456', 'an.nguyen@example.com', '123 Le Loi, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Tran', 'Binh', '0906123457', 'binh.tran@example.com', '45 Nguyen Trai, Q5, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Le', 'Cuong', '0907123458', 'cuong.le@example.com', '89 Hai Ba Trung, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Pham', 'Dung', '0908123459', 'dung.pham@example.com', '12 Cach Mang Thang 8, Q10, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Hoang', 'Em', '0909123460', 'em.hoang@example.com', '34 Tran Hung Dao, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Do', 'Phuc', '0910123461', 'phuc.do@example.com', '56 Vo Van Tan, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Bui', 'Giang', '0911123462', 'giang.bui@example.com', '78 Ly Thuong Kiet, Q10, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Dang', 'Hoa', '0912123463', 'hoa.dang@example.com', '90 Pasteur, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Nguyen', 'Hieu', '0913123464', 'hieu.nguyen@example.com', '101 Nguyen Dinh Chieu, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Tran', 'Khanh', '0914123465', 'khanh.tran@example.com', '23 Nguyen Van Cu, Q5, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Le', 'Lan', '0915123466', 'lan.le@example.com', '78 Dinh Tien Hoang, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Pham', 'Minh', '0916123467', 'minh.pham@example.com', '34 Nguyen Huu Canh, Binh Thanh');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Hoang', 'Nga', '0917123468', 'nga.hoang@example.com', '56 Dien Bien Phu, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Do', 'Oanh', '0918123469', 'oanh.do@example.com', '67 Vo Thi Sau, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Bui', 'Phat', '0919123470', 'phat.bui@example.com', '89 Nguyen Thi Minh Khai, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Dang', 'Quyen', '0920123471', 'quyen.dang@example.com', '120 Hoang Van Thu, Phu Nhuan');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Nguyen', 'Son', '0921123472', 'son.nguyen@example.com', '230 Le Van Sy, Tan Binh');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Tran', 'Tam', '0922123473', 'tam.tran@example.com', '88 Nam Ky Khoi Nghia, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Le', 'Uyen', '0923123474', 'uyen.le@example.com', '12 Phan Dinh Phung, Phu Nhuan');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Pham', 'Vy', '0924123475', 'vy.pham@example.com', '44 Truong Dinh, Q3, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Hoang', 'Xuan', '0925123476', 'xuan.hoang@example.com', '35 Le Duan, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Do', 'Yen', '0926123477', 'yen.do@example.com', '99 Nguyen Thai Hoc, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Bui', 'Anh', '0927123478', 'anh.bui@example.com', '23 Pham Viet Chanh, Binh Thanh');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Dang', 'Bach', '0928123479', 'bach.dang@example.com', '14 Phan Xich Long, Phu Nhuan');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Nguyen', 'Chi', '0929123480', 'chi.nguyen@example.com', '77 Cach Mang Thang 8, Q10');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Tran', 'Dao', '0930123481', 'dao.tran@example.com', '112 Ly Tu Trong, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Le', 'Giang', '0931123482', 'giang.le@example.com', '56 Pham Ngu Lao, Q1, HCM');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Pham', 'Hanh', '0932123483', 'hanh.pham@example.com', '78 Nguyen Van Troi, Phu Nhuan');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Hoang', 'Kien', '0933123484', 'kien.hoang@example.com', '89 Hoang Hoa Tham, Binh Thanh');
INSERT INTO customer (last_name, first_name, phone_number, email, address) VALUES ('Do', 'Linh', '0934123485', 'linh.do@example.com', '101 Tan Son Nhi, Tan Phu');

COMMIT;





INSERT INTO orders (  customer_id, order_date, total_amount) VALUES (2, TO_DATE('2025-04-24', 'YYYY-MM-DD'), 1200.50);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 2, TO_DATE('2025-04-24', 'YYYY-MM-DD'), 1200.50);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 3, TO_DATE('2025-04-23', 'YYYY-MM-DD'), 1500.75);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 4, TO_DATE('2025-04-22', 'YYYY-MM-DD'), 950.25);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 5, TO_DATE('2025-04-21', 'YYYY-MM-DD'), 1100.00);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 6, TO_DATE('2025-04-20', 'YYYY-MM-DD'), 2000.40);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 7, TO_DATE('2025-04-19', 'YYYY-MM-DD'), 1450.30);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 8, TO_DATE('2025-04-18', 'YYYY-MM-DD'), 1325.60);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 9, TO_DATE('2025-04-17', 'YYYY-MM-DD'), 890.75);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 10, TO_DATE('2025-04-16', 'YYYY-MM-DD'), 1675.80);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 11, TO_DATE('2025-04-15', 'YYYY-MM-DD'), 2005.25);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 12, TO_DATE('2025-04-14', 'YYYY-MM-DD'), 980.00);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 13, TO_DATE('2025-04-13', 'YYYY-MM-DD'), 1520.40);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 14, TO_DATE('2025-04-12', 'YYYY-MM-DD'), 1420.55);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 15, TO_DATE('2025-04-11', 'YYYY-MM-DD'), 1105.20);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 16, TO_DATE('2025-04-10', 'YYYY-MM-DD'), 1890.00);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 17, TO_DATE('2025-04-09', 'YYYY-MM-DD'), 1345.10);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 18, TO_DATE('2025-04-08', 'YYYY-MM-DD'), 1250.80);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 19, TO_DATE('2025-04-07', 'YYYY-MM-DD'), 1095.50);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 20, TO_DATE('2025-04-06', 'YYYY-MM-DD'), 1050.00);

INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 21, TO_DATE('2025-04-05', 'YYYY-MM-DD'), 1115.40);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 22, TO_DATE('2025-04-04', 'YYYY-MM-DD'), 1215.60);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 23, TO_DATE('2025-04-03', 'YYYY-MM-DD'), 980.20);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 24, TO_DATE('2025-04-02', 'YYYY-MM-DD'), 1150.30);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 25, TO_DATE('2025-04-01', 'YYYY-MM-DD'), 1260.00);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 26, TO_DATE('2025-03-31', 'YYYY-MM-DD'), 1340.25);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 27, TO_DATE('2025-03-30', 'YYYY-MM-DD'), 1210.90);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 28, TO_DATE('2025-03-29', 'YYYY-MM-DD'), 1455.10);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 29, TO_DATE('2025-03-28', 'YYYY-MM-DD'), 1090.60);
INSERT INTO orders (  customer_id, order_date, total_amount) VALUES ( 30, TO_DATE('2025-03-27', 'YYYY-MM-DD'), 1300.75);

commit;



INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Food', 'Food', 15.99, 100);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Toy', 'Toys', 9.99, 150);
INSERT INTO product (product_name, category, price, stock) VALUES ('Cat Collar', 'Accessories', 5.50, 80);
INSERT INTO product (product_name, category, price, stock) VALUES ('Bird Cage', 'Cages', 50.00, 30);
INSERT INTO product (product_name, category, price, stock) VALUES ('Fish Tank', 'Aquarium', 120.00, 25);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Leash', 'Accessories', 7.99, 100);
INSERT INTO product (product_name, category, price, stock) VALUES ('Cat Litter', 'Litter', 20.00, 200);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Shampoo', 'Grooming', 8.99, 120);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Bed', 'Furniture', 30.00, 60);
INSERT INTO product (product_name, category, price, stock) VALUES ('Fish Food', 'Food', 12.50, 180);
INSERT INTO product (product_name, category, price, stock) VALUES ('Hamster Wheel', 'Toys', 5.99, 200);
INSERT INTO product (product_name, category, price, stock) VALUES ('Reptile Heat Lamp', 'Reptile', 25.00, 50);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Bowl', 'Accessories', 3.50, 200);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Carrier', 'Accessories', 45.00, 75);
INSERT INTO product (product_name, category, price, stock) VALUES ('Bird Food', 'Food', 10.00, 150);
INSERT INTO product (product_name, category, price, stock) VALUES ('Cat Scratching Post', 'Furniture', 22.50, 85);
INSERT INTO product (product_name, category, price, stock) VALUES ('Rabbit Hutch', 'Cages', 60.00, 40);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Sweater', 'Clothing', 15.00, 100);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Brush', 'Grooming', 6.00, 200);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Teeth Cleaner', 'Health', 8.50, 90);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Boots', 'Clothing', 12.00, 70);
INSERT INTO product (product_name, category, price, stock) VALUES ('Fish Filter', 'Aquarium', 35.00, 50);
INSERT INTO product (product_name, category, price, stock) VALUES ('Hamster Cage', 'Cages', 40.00, 60);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Muzzle', 'Accessories', 9.00, 100);
INSERT INTO product (product_name, category, price, stock) VALUES ('Cat Toy', 'Toys', 4.00, 150);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Medicine', 'Health', 20.00, 75);
INSERT INTO product (product_name, category, price, stock) VALUES ('Bird Cage Stand', 'Cages', 30.00, 30);
INSERT INTO product (product_name, category, price, stock) VALUES ('Reptile Water Dish', 'Reptile', 10.00, 80);
INSERT INTO product (product_name, category, price, stock) VALUES ('Pet Poop Bags', 'Accessories', 2.99, 250);
INSERT INTO product (product_name, category, price, stock) VALUES ('Cat House', 'Furniture', 28.00, 40);
INSERT INTO product (product_name, category, price, stock) VALUES ('Dog Training Pads', 'Accessories', 12.00, 100);
commit;












---------------------------------------------------------------------STORED PROCEDURES-------------------------------------------------------------------


-- Thêm thú cung
CREATE OR REPLACE PROCEDURE add_pet (
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

    INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, stock)
    VALUES (p_pet_name, p_pet_type, p_breed, p_age, p_gender, p_price, p_stock);
    

    p_message := 'Them thu cung thanh cong!';

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'ERROR: ' || SQLERRM;
END;
/


--Thêm khách hàng
CREATE OR REPLACE PROCEDURE add_customer(
    p_first_name    IN VARCHAR2,
    p_last_name     IN VARCHAR2,
    p_phone_number  IN VARCHAR2,
    p_email         IN VARCHAR2,
    p_address       IN VARCHAR2,
    p_message       OUT VARCHAR2  
) IS
    v_count_phone INT;
    v_count_email INT;
    invalid_format EXCEPTION;
    duplicate_data EXCEPTION;
BEGIN
    -- Kiểm tra định dạng số điện thoại: bắt đầu bằng 0 và có 10 chữ số (VN)
    IF NOT REGEXP_LIKE(p_phone_number, '^0\d{9}$') THEN
        RAISE invalid_format;
    END IF;

    -- Kiểm tra định dạng email
    IF NOT REGEXP_LIKE(p_email, '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$') THEN
        RAISE invalid_format;
    END IF;

    -- Kiểm tra trùng số điện thoại
    SELECT COUNT(*) INTO v_count_phone
    FROM customer
    WHERE phone_number = p_phone_number;

    IF v_count_phone > 0 THEN
        p_message := 'Lỗi: Số điện thoại đã tồn tại.';
        RETURN;
    END IF;

    -- Kiểm tra trùng email
    SELECT COUNT(*) INTO v_count_email
    FROM customer
    WHERE email = p_email;

    IF v_count_email > 0 THEN
        p_message := 'Lỗi: Email đã tồn tại.';
        RETURN;
    END IF;

    -- Chèn khách hàng
    INSERT INTO customer (first_name, last_name, phone_number, email, address)
    VALUES (p_first_name, p_last_name, p_phone_number, p_email, p_address);

    COMMIT;
    p_message := 'Them khach hang thanh cong!';

EXCEPTION
    WHEN invalid_format THEN
        p_message := 'Lỗi: Số điện thoại hoặc email không hợp lệ.';
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/





-- Thêm sản phẩm
CREATE OR REPLACE PROCEDURE add_product(
    p_product_name  IN VARCHAR2,
    p_category      IN VARCHAR2,
    p_price         IN DECIMAL,
    p_stock         IN INT,
    p_message       OUT VARCHAR2
) IS
BEGIN
    INSERT INTO product (product_name, category, price, stock)
    VALUES (p_product_name, p_category, p_price, p_stock);
    
    COMMIT;
    p_message := 'Them san pham thanh cong!';
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        p_message := 'Lỗi: Mã sản phẩm đã tồn tại.';
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/







-- Thêm don hàng
CREATE OR REPLACE PROCEDURE add_order(
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
        p_message := 'Lỗii: Mã khách hàng không tồn tại.';
        RETURN;
    END IF;


    INSERT INTO orders (customer_id, order_date, total_amount)
    VALUES (p_customer_id, p_order_date, p_total_amount);

    COMMIT;
    p_message := 'Them hoa don thanh cong!';
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/




-- S?a thông tin thú cung
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
        p_message := 'L?i: Không tìm th?y thú cung v?i mã ID ' || p_pet_id;
    END IF;

    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
        ROLLBACK; 
END;
/


-- S?a thông tin khách hàng
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
        p_message := 'L?i: Không tìm th?y khách hàng v?i mã ID ' || p_customer_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat thong tin khach hang thanh cong!';
    END IF;
    
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/


-- S?a thông tin s?n ph?m
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
        p_message := 'L?i: Không tìm th?y s?n ph?m v?i mã ID ' || p_product_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat thong tin san pham thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/


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
        p_message := 'L?i: Mã khách hàng ' || p_customer_id || ' không t?n t?i.';
        RETURN;
    END IF;


    UPDATE orders
    SET customer_id = p_customer_id,
        order_date = p_order_date,
        total_amount = p_total_amount
    WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'L?i: Không tìm th?y don hàng v?i mã ID ' || p_order_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat don hang thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/




-- Xóa thú cung
CREATE OR REPLACE PROCEDURE delete_pet(
    p_pet_id IN INT,
    p_message OUT VARCHAR2 
) IS
BEGIN
    DELETE FROM pet WHERE pet_id = p_pet_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'L?i: Không tìm th?y thú cung v?i mã ID ' || p_pet_id;
    ELSE
        COMMIT;
        p_message := 'Xoa thu cung thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
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
        p_message := 'L?i: Không tìm th?y khách hàng v?i mã ID ' || p_customer_id;
    ELSE
        COMMIT;
        p_message := 'Xoa khach hang thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/


CREATE OR REPLACE PROCEDURE delete_product(
    p_product_id IN INT,
    p_message    OUT VARCHAR2
) IS
BEGIN
    DELETE FROM product WHERE product_id = p_product_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'L?i: Không tìm th?y s?n ph?m v?i mã ID ' || p_product_id;
    ELSE
        COMMIT;
        p_message := 'Xoa san pham thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/


-- Xóa don hàng
CREATE OR REPLACE PROCEDURE delete_order(
    p_order_id IN INT,
    p_message OUT VARCHAR2
) IS
BEGIN
    DELETE FROM orders WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'L?i: Không tìm th?y don hàng v?i mã ID ' || p_order_id;
    ELSE
        COMMIT;
        p_message := 'Xoa don hang thanh cong!';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'L?i không xác d?nh: ' || SQLERRM;
END;
/
