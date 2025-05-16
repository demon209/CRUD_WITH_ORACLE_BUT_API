
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
    gender VARCHAR2(50),
    price DECIMAL(10,2),
    status NVARCHAR2(200) DEFAULT 'Còn thú cưng',
    image blob NULL
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
ALTER TABLE product ADD product_type VARCHAR2(20); -- 'product' hoặc 'service'

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
    pet_id INT null,
    product_id INT null,
    order_date DATE,
    total_amount DECIMAL(10,2),
    FOREIGN KEY (customer_id) REFERENCES customer(customer_id),
    FOREIGN KEY (pet_id) REFERENCES pet(pet_id),
    FOREIGN KEY (product_id) REFERENCES product(product_id)
);
ALTER TABLE orders ADD quantity INT null;

CREATE OR REPLACE TRIGGER trg_orders_id
BEFORE INSERT ON orders
FOR EACH ROW
BEGIN
    :NEW.order_id := seq_orders.NEXTVAL;
END;
/



CREATE SEQUENCE seq_customer_pet START WITH 1 INCREMENT BY 1;

CREATE TABLE customer_pet (
    customer_pet_id INT PRIMARY KEY,
    customer_id INT NOT NULL,
    pet_name VARCHAR2(100) NOT NULL,
    product_id INT NOT NULL,
    product_name VARCHAR2(255),
    status NVARCHAR2(200) DEFAULT 'Đang thực hiện',

    CONSTRAINT fk_customer
        FOREIGN KEY (customer_id) REFERENCES customer(customer_id),

    CONSTRAINT fk_product
        FOREIGN KEY (product_id) REFERENCES product(product_id)
);
ALTER TABLE customer_pet
ADD (first_name VARCHAR2(100),
     last_name VARCHAR2(100));


CREATE OR REPLACE TRIGGER trg_auto_fill_customer_pet
BEFORE INSERT ON customer_pet
FOR EACH ROW
DECLARE
    v_first_name customer.first_name%TYPE;
    v_last_name  customer.last_name%TYPE;
    v_product_name  product.product_name%TYPE;
BEGIN
    -- Lấy first name và last name từ bảng customer
    SELECT first_name, last_name INTO v_first_name, v_last_name
    FROM customer
    WHERE customer_id = :NEW.customer_id;

    -- Lấy tên sản phẩm từ bảng product
    SELECT product_name INTO v_product_name
    FROM product
    WHERE product_id = :NEW.product_id;

    -- Gán vào bản ghi mới
    :NEW.first_name := v_first_name;
    :NEW.last_name := v_last_name;
    :NEW.product_name := v_product_name;
END;
/
CREATE OR REPLACE TRIGGER trg_customer_pet_id
BEFORE INSERT ON customer_pet
FOR EACH ROW
BEGIN
    :NEW.customer_pet_id := seq_customer_pet.NEXTVAL;
END;
/











DROP TABLE orders CASCADE CONSTRAINTS;
DROP TABLE pet CASCADE CONSTRAINTS;
DROP TABLE customer_pet CASCADE CONSTRAINTS;
DROP TABLE customer CASCADE CONSTRAINTS;
DROP TABLE product CASCADE CONSTRAINTS;

DROP SEQUENCE seq_product;
DROP SEQUENCE seq_pet;
DROP SEQUENCE seq_orders;
DROP SEQUENCE seq_customer_pet;
DROP SEQUENCE seq_customer;




---------------------------------------------------------------------INSERT DATA-------------------------------------------------------------



INSERT INTO pet (pet_name, pet_type, breed, age, gender, price) VALUES
('Bobby', 'Chó', 'Poodle', 2, 'Đực', 4500000);

INSERT INTO pet (pet_name,  pet_type, breed, age, gender, price ) VALUES
('Mimi', 'Mèo', 'Mèo Anh Lông Ngắn', 1, 'Cái', 3000000);

INSERT INTO pet (pet_name,  pet_type, breed, age, gender, price ) VALUES
('Whiskers', 'Mèo', 'Mèo Ba Tư', 2, 'Đực', 5000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Rex', 'Chó', 'Golden Retriever', 3, 'Đực', 6000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Luna', 'Mèo', 'Mèo Xiêm', 2, 'Cái', 3500000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Rocky', 'Chó', 'Bulldog', 4, 'Đực', 7000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Coco', 'Chó', 'Chihuahua', 1, 'Cái', 3200000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Snowy', 'Mèo', 'Mèo Ragdoll', 2, 'Cái', 5500000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Max', 'Chó', 'Labrador', 3, 'Đực', 5800000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Kitty', 'Mèo', 'Mèo Munchkin', 1, 'Cái', 4000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Benji', 'Chó', 'Pug', 2, 'Đực', 3800000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Simba', 'Mèo', 'Mèo Maine Coon', 4, 'Đực', 6200000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Daisy', 'Chó', 'Cocker Spaniel', 3, 'Cái', 4700000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Leo', 'Chó', 'Shih Tzu', 2, 'Đực', 3500000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Chloe', 'Mèo', 'Mèo Anh Lông Dài', 3, 'Cái', 5200000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Oscar', 'Chó', 'Samoyed', 1, 'Đực', 8000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Bella', 'Mèo', 'Mèo Scottish Fold', 2, 'Cái', 4800000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Teddy', 'Chó', 'Bichon Frise', 1, 'Đực', 4200000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Lily', 'Mèo', 'Mèo Sphynx', 2, 'Cái', 7500000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Bruno', 'Chó', 'Doberman', 3, 'Đực', 9000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Angel', 'Mèo', 'Mèo Bengal', 1, 'Cái', 6700000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Shadow', 'Chó', 'Husky', 2, 'Đực', 7500000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Ruby', 'Mèo', 'Mèo Mỹ Tai Cụp', 3, 'Cái', 5300000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Zorro', 'Chó', 'Beagle', 2, 'Đực', 3900000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Lola', 'Mèo', 'Mèo Himalaya', 2, 'Cái', 5600000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Thor', 'Chó', 'Rottweiler', 3, 'Đực', 8200000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Zoe', 'Mèo', 'Mèo Somali', 2, 'Cái', 6000000);

INSERT INTO pet (pet_name, pet_type, breed, age, gender, price ) VALUES
('Spike', 'Chó', 'Chó Phú Quốc', 4, 'Đực', 5000000);


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





INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (1, 2, 15, 27, TO_DATE('2025-04-24', 'YYYY-MM-DD'), 1200.50);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (2, 3, 8, 35, TO_DATE('2025-04-23', 'YYYY-MM-DD'), 1500.75);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (3, 4, 22, 13, TO_DATE('2025-04-22', 'YYYY-MM-DD'), 950.25);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (4, 5, 33, 41, TO_DATE('2025-04-21', 'YYYY-MM-DD'), 1100.00);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (5, 6, 19, 7, TO_DATE('2025-04-20', 'YYYY-MM-DD'), 2000.40);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (6, 7, 1, 49, TO_DATE('2025-04-19', 'YYYY-MM-DD'), 1450.30);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (7, 8, 26, 16, TO_DATE('2025-04-18', 'YYYY-MM-DD'), 1325.60);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (8, 9, 47, 2, TO_DATE('2025-04-17', 'YYYY-MM-DD'), 890.75);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (9, 10, 10, 25, TO_DATE('2025-04-16', 'YYYY-MM-DD'), 1675.80);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (10, 11, 36, 14, TO_DATE('2025-04-15', 'YYYY-MM-DD'), 2005.25);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (11, 12, 3, 38, TO_DATE('2025-04-14', 'YYYY-MM-DD'), 980.00);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (12, 13, 48, 22, TO_DATE('2025-04-13', 'YYYY-MM-DD'), 1520.40);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (13, 14, 6, 12, TO_DATE('2025-04-12', 'YYYY-MM-DD'), 1420.55);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (14, 15, 29, 30, TO_DATE('2025-04-11', 'YYYY-MM-DD'), 1105.20);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (15, 16, 18, 9, TO_DATE('2025-04-10', 'YYYY-MM-DD'), 1890.00);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (16, 17, 43, 5, TO_DATE('2025-04-09', 'YYYY-MM-DD'), 1345.10);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (17, 18, 40, 33, TO_DATE('2025-04-08', 'YYYY-MM-DD'), 1250.80);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (18, 19, 25, 18, TO_DATE('2025-04-07', 'YYYY-MM-DD'), 1095.50);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (19, 20, 12, 45, TO_DATE('2025-04-06', 'YYYY-MM-DD'), 1050.00);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (20, 21, 5, 10, TO_DATE('2025-04-05', 'YYYY-MM-DD'), 1115.40);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (21, 22, 32, 6, TO_DATE('2025-04-04', 'YYYY-MM-DD'), 1215.60);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (22, 23, 11, 39, TO_DATE('2025-04-03', 'YYYY-MM-DD'), 980.20);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (23, 24, 30, 17, TO_DATE('2025-04-02', 'YYYY-MM-DD'), 1150.30);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (24, 25, 44, 28, TO_DATE('2025-04-01', 'YYYY-MM-DD'), 1260.00);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (25, 26, 14, 4, TO_DATE('2025-03-31', 'YYYY-MM-DD'), 1340.25);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (26, 27, 35, 36, TO_DATE('2025-03-30', 'YYYY-MM-DD'), 1210.90);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (27, 28, 21, 8, TO_DATE('2025-03-29', 'YYYY-MM-DD'), 1455.10);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (28, 29, 9, 1, TO_DATE('2025-03-28', 'YYYY-MM-DD'), 1090.60);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (29, 30, 24, 50, TO_DATE('2025-03-27', 'YYYY-MM-DD'), 1300.75);
INSERT INTO orders (order_id, customer_id, pet_id, product_id, order_date, total_amount) VALUES (30, 31, 17, 20, TO_DATE('2025-03-26', 'YYYY-MM-DD'), 1185.90);

COMMIT;




INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Food', 'Food', 390000, 100, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Toy', 'Toys', 245000, 150, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Cat Collar', 'Accessories', 135000, 80, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Bird Cage', 'Cages', 1225000, 30, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Fish Tank', 'Aquarium', 2950000, 25, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Leash', 'Accessories', 196000, 100, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Cat Litter', 'Litter', 490000, 200, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Bed', 'Furniture', 735000, 60, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Fish Food', 'Food', 305000, 180, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Hamster Wheel', 'Toys', 147000, 200, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Reptile Heat Lamp', 'Reptile', 612000, 50, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Bowl', 'Accessories', 86000, 200, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Carrier', 'Accessories', 1100000, 75, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Bird Food', 'Food', 245000, 150, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Cat Scratching Post', 'Furniture', 553000, 85, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Rabbit Hutch', 'Cages', 1470000, 40, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Sweater', 'Clothing', 367000, 100, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Boots', 'Clothing', 294000, 70, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Fish Filter', 'Aquarium', 857000, 50, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Hamster Cage', 'Cages', 980000, 60, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Muzzle', 'Accessories', 221000, 100, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Cat Toy', 'Toys', 98000, 150, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Bird Cage Stand', 'Cages', 735000, 30, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Reptile Water Dish', 'Reptile', 245000, 80, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Poop Bags', 'Accessories', 74000, 250, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Cat House', 'Furniture', 686000, 40, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Dog Training Pads', 'Accessories', 294000, 100, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Medicine', 'Health', 490000, 75, 'product');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Shampoo', 'Grooming', 220000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Grooming - Full', 'Grooming', 612000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Nail Trimming', 'Beauty', 245000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Fur Trimming', 'Beauty', 368000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Teeth Cleaning', 'Health', 209000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Massage', 'Wellness', 490000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Vaccination Package', 'Health', 857000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Pet Spa', 'Beauty', 980000, 0, 'service');
INSERT INTO product (product_name, category, price, stock, product_type) VALUES ('Medical Checkup', 'Health', 735000, 0, 'service');

commit;







-------------------------------------------------------------------SELECT-----------------------------------------------------------------------------------

SELECT a.customer_pet_id, a.customer_id,customer.first_name,customer.last_name, pet_name, a.purchase_date,
       a.price_at_purchase
  FROM customer_pet a join customer on a.customer_id=customer.customer_id;



SELECT a.pet_id, a.pet_name, a.pet_type, a.breed, a.age, a.gender,
       a.price, a.status, a.image
  FROM pet a;



























---------------------------------------------------------------------STORED PROCEDURES-------------------------------------------------------------------


-- Thêm thú cung
CREATE OR REPLACE PROCEDURE add_pet (
    p_pet_name IN VARCHAR2,
    p_pet_type IN VARCHAR2,
    p_breed IN VARCHAR2,
    p_age IN INT,
    p_gender IN VARCHAR2,
    p_price IN DECIMAL,
    p_image IN BLOB,  -- Thêm tham số cho ảnh
    p_message OUT VARCHAR2
) IS
BEGIN
    -- Chèn dữ liệu vào bảng pet, bao gồm ảnh (kiểu dữ liệu BLOB)
    INSERT INTO pet (pet_name, pet_type, breed, age, gender, price, status, image)
    VALUES (p_pet_name, p_pet_type, p_breed, p_age, p_gender, p_price,'Còn thú cưng', p_image);

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
    p_type          IN VARCHAR2,
    p_message       OUT VARCHAR2
) IS
BEGIN
    INSERT INTO product (product_name, category, price, stock, product_type)
    VALUES (p_product_name, p_category, p_price, p_stock, p_type);
    
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
    p_customer_id   IN INT,
    p_pet_id        IN INT,
    p_product_id    IN INT,
    p_quantity      IN INT,
    p_order_date    IN DATE,
    p_total_amount  OUT DECIMAL,
    p_message       OUT VARCHAR2
) IS
    v_count      INT;
    v_pet_price  DECIMAL(10,2) := 0;
    v_pro_price  DECIMAL(10,2) := 0;
    v_stock      INT := 0;
BEGIN
    -- Kiểm tra customer
    SELECT COUNT(*) INTO v_count FROM customer WHERE customer_id = p_customer_id;
    IF v_count = 0 THEN
        p_message := 'Lỗi: Mã khách hàng không tồn tại.';
        RETURN;
    END IF;

    -- Xử lý pet nếu có
    IF p_pet_id IS NOT NULL THEN
        SELECT COUNT(*), NVL(MAX(price), 0) INTO v_count, v_pet_price FROM pet WHERE pet_id = p_pet_id;
        IF v_count = 0 THEN
            p_message := 'Lỗi: Mã thú cưng không tồn tại.';
            RETURN;
        END IF;

        -- Cập nhật trạng thái thú cưng thành "Đã bán"
        UPDATE pet SET status = 'Đã bán' WHERE pet_id = p_pet_id;
    END IF;

    -- Xử lý sản phẩm nếu có
    IF p_product_id IS NOT NULL THEN
        SELECT COUNT(*), NVL(MAX(price), 0), NVL(MAX(stock), 0) 
        INTO v_count, v_pro_price, v_stock 
        FROM product WHERE product_id = p_product_id;

        IF v_count = 0 THEN
            p_message := 'Lỗi: Mã sản phẩm không tồn tại.';
            RETURN;
        END IF;

        IF p_quantity IS NULL OR p_quantity <= 0 THEN
            p_message := 'Lỗi: Số lượng sản phẩm không hợp lệ.';
            RETURN;
        END IF;

        IF p_quantity > v_stock THEN
            p_message := 'Lỗi: Sản phẩm không đủ hàng trong kho.';
            RETURN;
        END IF;

        -- Trừ tồn kho
        UPDATE product SET stock = stock - p_quantity WHERE product_id = p_product_id;
    END IF;

    -- Tính tổng tiền
    p_total_amount := v_pet_price + (v_pro_price * NVL(p_quantity, 0));

    -- Thêm đơn hàng
    INSERT INTO orders (customer_id, pet_id, product_id, quantity, order_date, total_amount)
    VALUES (p_customer_id, p_pet_id, p_product_id, p_quantity, p_order_date, p_total_amount);

    COMMIT;
    p_message := 'Them don hang thanh cong!';
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/






CREATE OR REPLACE PROCEDURE update_pet(
    p_pet_id IN INT,
    p_pet_name IN VARCHAR2,
    p_pet_type IN VARCHAR2,
    p_breed IN VARCHAR2,
    p_age IN INT,
    p_gender IN VARCHAR2,
    p_price IN DECIMAL,
    p_status in nvarchar2,
    p_image IN BLOB,  -- Thêm tham số ảnh
    p_message OUT VARCHAR2
) IS
BEGIN
    -- Cập nhật thông tin thú cưng (các trường khác ngoài ảnh)
    UPDATE pet
    SET pet_name = p_pet_name,
        pet_type = p_pet_type,
        breed = p_breed,
        age = p_age,
        gender = p_gender,
        price = p_price,
        status = 'Còn thú cưng',
        -- Cập nhật ảnh nếu có, nếu không thì giữ ảnh cũ
        image = COALESCE(p_image, image)
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
    p_product_type in varchar2,
    p_message      OUT VARCHAR2
) IS
BEGIN
    UPDATE product
    SET product_name = p_product_name,
        category     = p_category,
        price        = p_price,
        stock        = p_stock,
        product_type = p_product_type
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
    p_order_id      IN INT,
    p_customer_id   IN INT,
    p_pet_id        IN INT,
    p_product_id    IN INT,
    p_quantity      IN INT,
    p_order_date    IN DATE,
    p_total_amount  OUT DECIMAL,
    p_message       OUT VARCHAR2
) IS
    v_count       INT;
    v_pet_price   DECIMAL(10,2) := 0;
    v_pro_price   DECIMAL(10,2) := 0;
    v_old_qty     INT := 0;
    v_stock       INT := 0;
    v_old_pet_id  INT;
BEGIN
    -- Kiểm tra customer
    SELECT COUNT(*) INTO v_count FROM customer WHERE customer_id = p_customer_id;
    IF v_count = 0 THEN
        p_message := 'Lỗi: Mã khách hàng không tồn tại.';
        RETURN;
    END IF;

    -- Xử lý pet nếu có
    IF p_pet_id IS NOT NULL THEN
        SELECT COUNT(*), NVL(MAX(price), 0) INTO v_count, v_pet_price FROM pet WHERE pet_id = p_pet_id;
        IF v_count = 0 THEN
            p_message := 'Lỗi: Mã thú cưng không tồn tại.';
            RETURN;
        END IF;
    END IF;

    -- Xử lý sản phẩm nếu có
    IF p_product_id IS NOT NULL THEN
        SELECT COUNT(*), NVL(MAX(price), 0), NVL(MAX(stock), 0) 
        INTO v_count, v_pro_price, v_stock 
        FROM product WHERE product_id = p_product_id;

        IF v_count = 0 THEN
            p_message := 'Lỗi: Mã sản phẩm không tồn tại.';
            RETURN;
        END IF;

        IF p_quantity IS NULL OR p_quantity <= 0 THEN
            p_message := 'Lỗi: Số lượng sản phẩm không hợp lệ.';
            RETURN;
        END IF;

        -- Lấy số lượng cũ
        SELECT quantity INTO v_old_qty FROM orders WHERE order_id = p_order_id;

        -- Cập nhật lại tồn kho: + số cũ, - số mới
        UPDATE product 
        SET stock = stock + NVL(v_old_qty, 0) - p_quantity 
        WHERE product_id = p_product_id;

        -- Kiểm tra tồn kho sau cập nhật
        SELECT stock INTO v_stock FROM product WHERE product_id = p_product_id;
        IF v_stock < 0 THEN
            ROLLBACK;
            p_message := 'Lỗi: Cập nhật vượt quá số lượng tồn kho.';
            RETURN;
        END IF;
    END IF;

    -- Tính tổng tiền
    p_total_amount := v_pet_price + (v_pro_price * NVL(p_quantity, 0));

    -- Cập nhật trạng thái thú cưng nếu thay đổi
-- Cập nhật trạng thái thú cưng nếu có sự thay đổi
SELECT pet_id INTO v_old_pet_id FROM orders WHERE order_id = p_order_id;

IF p_pet_id IS NOT NULL THEN
    -- Nếu có pet cũ và khác pet mới thì cập nhật lại trạng thái
    IF v_old_pet_id IS NOT NULL AND p_pet_id != v_old_pet_id THEN
        -- Trả lại trạng thái cũ cho pet trước đó
        UPDATE pet SET status = 'Còn thú cưng' WHERE pet_id = v_old_pet_id;
    END IF;

    -- Cập nhật trạng thái mới cho pet mới
    UPDATE pet SET status = 'Đã bán' WHERE pet_id = p_pet_id;
END IF;


    -- Cập nhật đơn hàng
    UPDATE orders
    SET customer_id  = p_customer_id,
        pet_id       = p_pet_id,
        product_id   = p_product_id,
        quantity     = p_quantity,
        order_date   = p_order_date,
        total_amount = p_total_amount
    WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy đơn hàng với ID ' || p_order_id;
    ELSE
        COMMIT;
        p_message := 'Cap nhat don hang thanh cong!';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_message := 'Lỗi không xác định: ' || SQLERRM;
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
    p_message  OUT VARCHAR2
) IS
    v_product_id INT;
    v_quantity   INT;
    v_pet_id     INT;
BEGIN
    -- Lấy thông tin sản phẩm, số lượng và pet_id từ đơn hàng
    SELECT product_id, quantity, pet_id
    INTO v_product_id, v_quantity, v_pet_id
    FROM orders
    WHERE order_id = p_order_id;

    -- Xóa đơn hàng
    DELETE FROM orders WHERE order_id = p_order_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Lỗi: Không tìm thấy đơn hàng với mã ID ' || p_order_id;
        RETURN;
    END IF;

    -- Hoàn trả số lượng vào kho nếu có product_id
    IF v_product_id IS NOT NULL THEN
        UPDATE product
        SET stock = stock + NVL(v_quantity, 0)
        WHERE product_id = v_product_id;
    END IF;

    -- Cập nhật lại trạng thái của thú cưng nếu có pet_id
    IF v_pet_id IS NOT NULL THEN
        UPDATE pet
        SET status = 'Còn thú cưng'
        WHERE pet_id = v_pet_id;
    END IF;

    COMMIT;
    p_message := 'Xoa don hang thanh cong!';
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        p_message := 'Lỗi: Không tìm thấy đơn hàng với mã ID ' || p_order_id;
    WHEN OTHERS THEN
        ROLLBACK;
        p_message := 'Lỗi không xác định: ' || SQLERRM;
END;
/


CREATE OR REPLACE PROCEDURE search_product (
    p_keyword IN VARCHAR2,
    p_result OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_result FOR
        SELECT product_id, product_name, category, price, stock
        FROM product
        WHERE LOWER(product_name) LIKE '%' || LOWER(p_keyword) || '%';
END;
/
CREATE OR REPLACE PROCEDURE search_customer (
    p_keyword IN VARCHAR2,
    p_result OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_result FOR
        SELECT customer_id, first_name, last_name, phone_number, email, address
        FROM customer
        WHERE LOWER(first_name) LIKE '%' || LOWER(p_keyword) || '%'
           OR LOWER(last_name) LIKE '%' || LOWER(p_keyword) || '%'
           OR LOWER(phone_number) LIKE '%' || LOWER(p_keyword) || '%'
           OR LOWER(email) LIKE '%' || LOWER(p_keyword) || '%';
END;
/
CREATE OR REPLACE PROCEDURE search_pet (
    p_keyword IN VARCHAR2,
    p_result OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_result FOR
        SELECT pet_id, pet_name, pet_type, breed, age, gender, price, status, image
        FROM pet
        WHERE LOWER(pet_name) LIKE '%' || LOWER(p_keyword) || '%'
           OR LOWER(pet_type) = LOWER(p_keyword)
           OR LOWER(breed) LIKE '%' || LOWER(p_keyword) || '%';
END;
/



CREATE OR REPLACE PROCEDURE search_order(
    p_keyword IN VARCHAR2,
    p_result OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_result FOR
        SELECT order_id,
               customer_id,
               pet_id,
               product_id,
               quantity,
               order_date,
               total_amount
        FROM orders
        WHERE TO_CHAR(order_date, 'YYYY-MM-DD') LIKE p_keyword
        ORDER BY order_id ASC;
END;
/

CREATE OR REPLACE PROCEDURE add_customerpet (
    p_customer_id     IN customer_pet.customer_id%TYPE,
    p_pet_name        IN customer_pet.pet_name%TYPE,
    p_product_id      IN customer_pet.product_id%TYPE,
    p_status          IN customer_pet.status%TYPE, -- thêm tham số status
    p_message         OUT VARCHAR2
) IS
BEGIN
    INSERT INTO customer_pet (
        customer_pet_id,
        customer_id,
        pet_name,
        product_id,
        status
    )
    VALUES (
        seq_customer_pet.NEXTVAL,
        p_customer_id,
        p_pet_name,
        p_product_id,
        p_status
    );

    p_message := 'Them thong tin dich vu thanh cong!';
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi khi thêm: ' || SQLERRM;
END;
/



CREATE OR REPLACE PROCEDURE delete_customerpet (
    p_customer_pet_id IN customer_pet.customer_pet_id%TYPE,
    p_message         OUT VARCHAR2
) IS
BEGIN
    DELETE FROM customer_pet
    WHERE customer_pet_id = p_customer_pet_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Không tìm thấy bản ghi để xóa.';
    ELSE
        p_message := 'Xoa thong tin dich vu thanh cong!';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi khi xóa: ' || SQLERRM;
END;
/
CREATE OR REPLACE PROCEDURE update_customerpet (
    p_customer_pet_id IN customer_pet.customer_pet_id%TYPE,
    p_customer_id     IN customer_pet.customer_id%TYPE,
    p_pet_name        IN customer_pet.pet_name%TYPE,
    p_product_id      IN customer_pet.product_id%TYPE,
    p_status          IN customer_pet.status%TYPE, -- thêm tham số status
    p_message         OUT VARCHAR2
) IS
BEGIN
    UPDATE customer_pet
    SET customer_id = p_customer_id,
        pet_name = p_pet_name,
        product_id = p_product_id,
        status = 'Đang thực hiện'
    WHERE customer_pet_id = p_customer_pet_id;

    IF SQL%ROWCOUNT = 0 THEN
        p_message := 'Không tìm thấy bản ghi để cập nhật.';
    ELSE
        p_message := 'Cap nhat thong tin thanh cong!';
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        p_message := 'Lỗi khi cập nhật: ' || SQLERRM;
END;
/


CREATE OR REPLACE PROCEDURE search_customerpet (
    p_keyword IN VARCHAR2,
    p_result  OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN p_result FOR
    SELECT cp.customer_pet_id,
           cp.customer_id,
           c.first_name,
           c.last_name,
           cp.pet_name,
           p.product_id,
           p.product_name,
           p.product_type,
           cp.status
    FROM customer_pet cp
    JOIN customer c ON cp.customer_id = c.customer_id
    JOIN product p ON cp.product_id = p.product_id
    WHERE LOWER(cp.pet_name) LIKE '%' || LOWER(p_keyword) || '%'
       OR LOWER(c.first_name) LIKE '%' || LOWER(p_keyword) || '%'
       OR LOWER(c.last_name) LIKE '%' || LOWER(p_keyword) || '%'
       OR TO_CHAR(cp.customer_pet_id) = p_keyword
       OR TO_CHAR(cp.customer_id) = p_keyword
       OR LOWER(p.product_name) LIKE '%' || LOWER(p_keyword) || '%';
END;



