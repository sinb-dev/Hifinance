
-- Contains the websites to be scraped
CREATE TABLE websites (
    website TEXT NOT NULL,
    created TEXT NOT NULL,
    base_url TEXT NOT NULL,
    PRIMARY KEY(website)
);

-- Contains categorised types of pages in a websites (eg. product, productlist, etc)
CREATE TABLE page_types
(
    page_type_id INTEGER,
    name TEXT NOT NULL,
    website TEXT,
    scrape_interval_hours INTEGER NOT NULL DEFAULT 24,
    identify_by_selector TEXT,
    FOREIGN KEY(website) REFERENCES websites(website) 
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    PRIMARY KEY(page_type_id AUTOINCREMENT)
);

-- Properties on a page to be scraped (eg productprice, inventory, description etc.)
CREATE TABLE target_properties(
	property_id INTEGER,
    property TEXT NOT NULL,
    data_type INTEGER NOT NULL,
    xpath TEXT NOT NULL DEFAULT '',
	selector TEXT NOT NULL DEFAULT '',
    page_type_id INTEGER,
    FOREIGN KEY(page_type_id) REFERENCES page_types(page_type_id) 
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    PRIMARY KEY(property_id AUTOINCREMENT),
	UNIQUE(property, page_type_id)
);

-- An actual URL on a website to be scraped
CREATE TABLE scrape_targets
(
    target_id INTEGER,
    url TEXT NOT NULL,
	next_visit TEXT,
    page_type_id INTEGER,
    last_http_status_code INTEGER NOT NULL,
    FOREIGN KEY(page_type_id) REFERENCES page_types(page_type_id) 
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    UNIQUE(url, page_type_id),
    PRIMARY KEY(target_id AUTOINCREMENT)
);


-- Values to properties on resources
CREATE TABLE target_data(
    property_id INTEGER,
    result TEXT,
    scraped TEXT,
	FOREIGN KEY(property_id) REFERENCES target_properties(property_id) 
        ON UPDATE CASCADE
        ON DELETE CASCADE,
	PRIMARY KEY(property_id, scraped)
);