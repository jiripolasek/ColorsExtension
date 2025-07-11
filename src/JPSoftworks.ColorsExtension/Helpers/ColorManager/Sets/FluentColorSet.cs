// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal sealed class FluentColorSet : IColorSet
{
    private readonly Dictionary<string, RgbColor> _colors
        = new(StringComparer.OrdinalIgnoreCase)
        {
            // Microsoft Fluent Design System Colors
            ["Anchor"] = (57, 65, 70),
            ["Anchor Shade 10"] = (51, 58, 63),
            ["Anchor Shade 20"] = (43, 49, 53),
            ["Anchor Shade 30"] = (32, 36, 39),
            ["Anchor Shade 40"] = (17, 19, 21),
            ["Anchor Shade 50"] = (9, 10, 11),
            ["Anchor Tint 10"] = (77, 86, 92),
            ["Anchor Tint 20"] = (98, 108, 114),
            ["Anchor Tint 30"] = (128, 138, 144),
            ["Anchor Tint 40"] = (188, 195, 199),
            ["Anchor Tint 50"] = (219, 223, 225),
            ["Anchor Tint 60"] = (246, 247, 248),

            ["Beige"] = (122, 117, 116),
            ["Beige Shade 10"] = (110, 105, 104),
            ["Beige Shade 20"] = (93, 89, 88),
            ["Beige Shade 30"] = (68, 66, 65),
            ["Beige Shade 40"] = (37, 35, 35),
            ["Beige Shade 50"] = (20, 19, 19),
            ["Beige Tint 10"] = (138, 133, 132),
            ["Beige Tint 20"] = (154, 149, 148),
            ["Beige Tint 30"] = (175, 171, 170),
            ["Beige Tint 40"] = (215, 212, 212),
            ["Beige Tint 50"] = (234, 232, 232),
            ["Beige Tint 60"] = (250, 249, 249),

            ["Berry"] = (194, 57, 179),
            ["Berry Shade 10"] = (175, 51, 161),
            ["Berry Shade 20"] = (147, 43, 136),
            ["Berry Shade 30"] = (109, 32, 100),
            ["Berry Shade 40"] = (58, 17, 54),
            ["Berry Shade 50"] = (31, 9, 29),
            ["Berry Tint 10"] = (201, 76, 188),
            ["Berry Tint 20"] = (209, 97, 196),
            ["Berry Tint 30"] = (218, 126, 208),
            ["Berry Tint 40"] = (237, 187, 231),
            ["Berry Tint 50"] = (245, 218, 242),
            ["Berry Tint 60"] = (253, 245, 252),

            ["Black"] = (0, 0, 0),

            ["Blue"] = (0, 120, 212),
            ["Blue Shade 10"] = (0, 108, 191),
            ["Blue Shade 20"] = (0, 91, 161),
            ["Blue Shade 30"] = (0, 67, 119),
            ["Blue Shade 40"] = (0, 36, 64),
            ["Blue Shade 50"] = (0, 19, 34),
            ["Blue Tint 10"] = (26, 134, 217),
            ["Blue Tint 20"] = (53, 149, 222),
            ["Blue Tint 30"] = (92, 170, 229),
            ["Blue Tint 40"] = (169, 211, 242),
            ["Blue Tint 50"] = (208, 231, 248),
            ["Blue Tint 60"] = (243, 249, 253),

            ["Brand-10"] = (0, 21, 38),
            ["Brand-100"] = (40, 153, 245),
            ["Brand-110"] = (58, 160, 243),
            ["Brand-120"] = (108, 184, 246),
            ["Brand-130"] = (130, 199, 255),
            ["Brand-140"] = (199, 224, 244),
            ["Brand-150"] = (222, 236, 249),
            ["Brand-160"] = (239, 246, 252),
            ["Brand-20"] = (0, 40, 72),
            ["Brand-30"] = (4, 56, 98),
            ["Brand-40"] = (0, 69, 120),
            ["Brand-50"] = (0, 76, 135),
            ["Brand-60"] = (0, 90, 158),
            ["Brand-70"] = (16, 110, 190),
            ["Brand-80"] = (0, 120, 212),
            ["Brand-90"] = (24, 144, 241),

            ["Brass"] = (152, 111, 11),
            ["Brass Shade 10"] = (137, 100, 10),
            ["Brass Shade 20"] = (116, 84, 8),
            ["Brass Shade 30"] = (85, 62, 6),
            ["Brass Shade 40"] = (46, 33, 3),
            ["Brass Shade 50"] = (24, 18, 2),
            ["Brass Tint 10"] = (164, 125, 30),
            ["Brass Tint 20"] = (177, 140, 52),
            ["Brass Tint 30"] = (193, 162, 86),
            ["Brass Tint 40"] = (224, 206, 162),
            ["Brass Tint 50"] = (239, 228, 203),
            ["Brass Tint 60"] = (251, 248, 242),

            ["Bronze"] = (167, 65, 9),
            ["Bronze Shade 10"] = (150, 58, 8),
            ["Bronze Shade 20"] = (127, 49, 7),
            ["Bronze Shade 30"] = (94, 36, 5),
            ["Bronze Shade 40"] = (50, 19, 3),
            ["Bronze Shade 50"] = (27, 10, 1),
            ["Bronze Tint 10"] = (178, 82, 30),
            ["Bronze Tint 20"] = (188, 101, 53),
            ["Bronze Tint 30"] = (202, 128, 87),
            ["Bronze Tint 40"] = (229, 187, 164),
            ["Bronze Tint 50"] = (241, 217, 204),
            ["Bronze Tint 60"] = (251, 245, 242),

            ["Brown"] = (142, 86, 46),
            ["Brown Shade 10"] = (128, 77, 41),
            ["Brown Shade 20"] = (108, 65, 35),
            ["Brown Shade 30"] = (80, 48, 26),
            ["Brown Shade 40"] = (43, 26, 14),
            ["Brown Shade 50"] = (23, 14, 7),
            ["Brown Tint 10"] = (156, 102, 63),
            ["Brown Tint 20"] = (169, 118, 82),
            ["Brown Tint 30"] = (187, 143, 111),
            ["Brown Tint 40"] = (221, 195, 176),
            ["Brown Tint 50"] = (237, 222, 211),
            ["Brown Tint 60"] = (250, 247, 244),

            ["Burgundy"] = (164, 38, 44),
            ["Burgundy Shade 10"] = (148, 34, 40),
            ["Burgundy Shade 20"] = (125, 29, 33),
            ["Burgundy Shade 30"] = (92, 21, 25),
            ["Burgundy Shade 40"] = (49, 11, 13),
            ["Burgundy Shade 50"] = (26, 6, 7),
            ["Burgundy Tint 10"] = (175, 57, 62),
            ["Burgundy Tint 20"] = (186, 77, 82),
            ["Burgundy Tint 30"] = (200, 108, 112),
            ["Burgundy Tint 40"] = (228, 175, 178),
            ["Burgundy Tint 50"] = (240, 211, 212),
            ["Burgundy Tint 60"] = (251, 244, 244),

            ["Charcoal"] = (57, 57, 57),
            ["Charcoal Shade 10"] = (51, 51, 51),
            ["Charcoal Shade 20"] = (43, 43, 43),
            ["Charcoal Shade 30"] = (32, 32, 32),
            ["Charcoal Shade 40"] = (17, 17, 17),
            ["Charcoal Shade 50"] = (9, 9, 9),
            ["Charcoal Tint 10"] = (81, 81, 81),
            ["Charcoal Tint 20"] = (104, 104, 104),
            ["Charcoal Tint 30"] = (136, 136, 136),
            ["Charcoal Tint 40"] = (196, 196, 196),
            ["Charcoal Tint 50"] = (223, 223, 223),
            ["Charcoal Tint 60"] = (247, 247, 247),

            ["Cornflower"] = (79, 107, 237),
            ["Cornflower Shade 10"] = (71, 96, 213),
            ["Cornflower Shade 20"] = (60, 81, 180),
            ["Cornflower Shade 30"] = (44, 60, 133),
            ["Cornflower Shade 40"] = (24, 32, 71),
            ["Cornflower Shade 50"] = (13, 17, 38),
            ["Cornflower Tint 10"] = (99, 124, 239),
            ["Cornflower Tint 20"] = (119, 141, 241),
            ["Cornflower Tint 30"] = (147, 164, 244),
            ["Cornflower Tint 40"] = (200, 209, 250),
            ["Cornflower Tint 50"] = (225, 230, 252),
            ["Cornflower Tint 60"] = (247, 249, 254),

            ["Cranberry"] = (197, 15, 31),
            ["Cranberry Shade 10"] = (177, 14, 28),
            ["Cranberry Shade 20"] = (150, 11, 24),
            ["Cranberry Shade 30"] = (110, 8, 17),
            ["Cranberry Shade 40"] = (59, 5, 9),
            ["Cranberry Shade 50"] = (32, 2, 5),
            ["Cranberry Tint 10"] = (204, 38, 53),
            ["Cranberry Tint 20"] = (211, 63, 76),
            ["Cranberry Tint 30"] = (220, 98, 109),
            ["Cranberry Tint 40"] = (238, 172, 178),
            ["Cranberry Tint 50"] = (246, 209, 213),
            ["Cranberry Tint 60"] = (253, 243, 244),

            ["Cyan"] = (0, 153, 188),
            ["Cyan Shade 10"] = (0, 138, 169),
            ["Cyan Shade 20"] = (0, 116, 143),
            ["Cyan Shade 30"] = (0, 86, 105),
            ["Cyan Shade 40"] = (0, 46, 56),
            ["Cyan Shade 50"] = (0, 24, 30),
            ["Cyan Tint 10"] = (24, 164, 196),
            ["Cyan Tint 20"] = (49, 175, 204),
            ["Cyan Tint 30"] = (86, 191, 215),
            ["Cyan Tint 40"] = (164, 222, 235),
            ["Cyan Tint 50"] = (205, 237, 244),
            ["Cyan Tint 60"] = (242, 250, 252),

            ["DarkBlue"] = (0, 57, 102),
            ["DarkBlue Shade 10"] = (0, 51, 92),
            ["DarkBlue Shade 20"] = (0, 43, 78),
            ["DarkBlue Shade 30"] = (0, 32, 57),
            ["DarkBlue Shade 40"] = (0, 17, 31),
            ["DarkBlue Shade 50"] = (0, 9, 16),
            ["DarkBlue Tint 10"] = (14, 74, 120),
            ["DarkBlue Tint 20"] = (33, 92, 139),
            ["DarkBlue Tint 30"] = (65, 120, 163),
            ["DarkBlue Tint 40"] = (146, 181, 209),
            ["DarkBlue Tint 50"] = (194, 214, 231),
            ["DarkBlue Tint 60"] = (239, 244, 249),

            ["DarkBrown"] = (77, 41, 28),
            ["DarkBrown Shade 10"] = (69, 37, 25),
            ["DarkBrown Shade 20"] = (58, 31, 21),
            ["DarkBrown Shade 30"] = (43, 23, 16),
            ["DarkBrown Shade 40"] = (23, 12, 8),
            ["DarkBrown Shade 50"] = (12, 7, 4),
            ["DarkBrown Tint 10"] = (98, 58, 43),
            ["DarkBrown Tint 20"] = (120, 77, 62),
            ["DarkBrown Tint 30"] = (148, 107, 92),
            ["DarkBrown Tint 40"] = (202, 173, 163),
            ["DarkBrown Tint 50"] = (227, 210, 203),
            ["DarkBrown Tint 60"] = (248, 243, 242),

            ["DarkGreen"] = (11, 106, 11),
            ["DarkGreen Shade 10"] = (10, 95, 10),
            ["DarkGreen Shade 20"] = (8, 81, 8),
            ["DarkGreen Shade 30"] = (6, 59, 6),
            ["DarkGreen Shade 40"] = (3, 32, 3),
            ["DarkGreen Shade 50"] = (2, 17, 2),
            ["DarkGreen Tint 10"] = (26, 124, 26),
            ["DarkGreen Tint 20"] = (45, 142, 45),
            ["DarkGreen Tint 30"] = (77, 166, 77),
            ["DarkGreen Tint 40"] = (154, 210, 154),
            ["DarkGreen Tint 50"] = (198, 231, 198),
            ["DarkGreen Tint 60"] = (240, 249, 240),

            ["DarkOrange"] = (218, 59, 1),
            ["DarkOrange Shade 10"] = (196, 53, 1),
            ["DarkOrange Shade 20"] = (166, 45, 1),
            ["DarkOrange Shade 30"] = (122, 33, 1),
            ["DarkOrange Shade 40"] = (65, 18, 0),
            ["DarkOrange Shade 50"] = (35, 9, 0),
            ["DarkOrange Tint 10"] = (222, 80, 28),
            ["DarkOrange Tint 20"] = (227, 101, 55),
            ["DarkOrange Tint 30"] = (233, 131, 94),
            ["DarkOrange Tint 40"] = (244, 191, 171),
            ["DarkOrange Tint 50"] = (249, 220, 209),
            ["DarkOrange Tint 60"] = (253, 246, 243),

            ["DarkPurple"] = (64, 27, 108),
            ["DarkPurple Shade 10"] = (58, 24, 97),
            ["DarkPurple Shade 20"] = (49, 21, 82),
            ["DarkPurple Shade 30"] = (36, 15, 60),
            ["DarkPurple Shade 40"] = (19, 8, 32),
            ["DarkPurple Shade 50"] = (10, 4, 17),
            ["DarkPurple Tint 10"] = (81, 43, 126),
            ["DarkPurple Tint 20"] = (99, 62, 143),
            ["DarkPurple Tint 30"] = (126, 92, 167),
            ["DarkPurple Tint 40"] = (185, 163, 211),
            ["DarkPurple Tint 50"] = (216, 204, 231),
            ["DarkPurple Tint 60"] = (245, 242, 249),

            ["DarkRed"] = (117, 11, 28),
            ["DarkRed Shade 10"] = (105, 10, 25),
            ["DarkRed Shade 20"] = (89, 8, 21),
            ["DarkRed Shade 30"] = (66, 6, 16),
            ["DarkRed Shade 40"] = (35, 3, 8),
            ["DarkRed Shade 50"] = (19, 2, 4),
            ["DarkRed Tint 10"] = (134, 27, 44),
            ["DarkRed Tint 20"] = (150, 47, 63),
            ["DarkRed Tint 30"] = (172, 79, 94),
            ["DarkRed Tint 40"] = (214, 156, 165),
            ["DarkRed Tint 50"] = (233, 199, 205),
            ["DarkRed Tint 60"] = (249, 240, 242),

            ["DarkTeal"] = (0, 102, 102),
            ["DarkTeal Shade 10"] = (0, 92, 92),
            ["DarkTeal Shade 20"] = (0, 78, 78),
            ["DarkTeal Shade 30"] = (0, 57, 57),
            ["DarkTeal Shade 40"] = (0, 31, 31),
            ["DarkTeal Shade 50"] = (0, 16, 16),
            ["DarkTeal Tint 10"] = (14, 120, 120),
            ["DarkTeal Tint 20"] = (33, 139, 139),
            ["DarkTeal Tint 30"] = (65, 163, 163),
            ["DarkTeal Tint 40"] = (146, 209, 209),
            ["DarkTeal Tint 50"] = (194, 231, 231),
            ["DarkTeal Tint 60"] = (239, 249, 249),

            ["Forest"] = (73, 130, 5),
            ["Forest Shade 10"] = (66, 117, 5),
            ["Forest Shade 20"] = (55, 99, 4),
            ["Forest Shade 30"] = (41, 73, 3),
            ["Forest Shade 40"] = (22, 39, 2),
            ["Forest Shade 50"] = (12, 21, 1),
            ["Forest Tint 10"] = (89, 145, 22),
            ["Forest Tint 20"] = (107, 160, 43),
            ["Forest Tint 30"] = (133, 180, 76),
            ["Forest Tint 40"] = (189, 217, 155),
            ["Forest Tint 50"] = (219, 235, 199),
            ["Forest Tint 60"] = (246, 250, 240),

            ["Gold"] = (193, 156, 0),
            ["Gold Shade 10"] = (174, 140, 0),
            ["Gold Shade 20"] = (147, 119, 0),
            ["Gold Shade 30"] = (108, 87, 0),
            ["Gold Shade 40"] = (58, 47, 0),
            ["Gold Shade 50"] = (31, 25, 0),
            ["Gold Tint 10"] = (200, 167, 24),
            ["Gold Tint 20"] = (208, 178, 50),
            ["Gold Tint 30"] = (218, 193, 87),
            ["Gold Tint 40"] = (236, 223, 165),
            ["Gold Tint 50"] = (245, 238, 206),
            ["Gold Tint 60"] = (253, 251, 242),

            ["Grape"] = (136, 23, 152),
            ["Grape Shade 10"] = (122, 21, 137),
            ["Grape Shade 20"] = (103, 17, 116),
            ["Grape Shade 30"] = (76, 13, 85),
            ["Grape Shade 40"] = (41, 7, 46),
            ["Grape Shade 50"] = (22, 4, 24),
            ["Grape Tint 10"] = (149, 42, 164),
            ["Grape Tint 20"] = (163, 63, 177),
            ["Grape Tint 30"] = (181, 95, 193),
            ["Grape Tint 40"] = (217, 167, 224),
            ["Grape Tint 50"] = (234, 206, 239),
            ["Grape Tint 60"] = (250, 242, 251),

            ["Green"] = (16, 124, 16),
            ["Green Shade 10"] = (14, 112, 14),
            ["Green Shade 20"] = (12, 94, 12),
            ["Green Shade 30"] = (9, 69, 9),
            ["Green Shade 40"] = (5, 37, 5),
            ["Green Shade 50"] = (3, 20, 3),
            ["Green Tint 10"] = (33, 140, 33),
            ["Green Tint 20"] = (53, 155, 53),
            ["Green Tint 30"] = (84, 176, 84),
            ["Green Tint 40"] = (159, 216, 159),
            ["Green Tint 50"] = (201, 234, 201),
            ["Green Tint 60"] = (241, 250, 241),

            ["Grey-0"] = (0, 0, 0),
            ["Grey-10"] = (26, 26, 26),
            ["Grey-100"] = (255, 255, 255),
            ["Grey-12"] = (31, 31, 31),
            ["Grey-14"] = (36, 36, 36),
            ["Grey-16"] = (41, 41, 41),
            ["Grey-18"] = (46, 46, 46),
            ["Grey-2"] = (5, 5, 5),
            ["Grey-20"] = (51, 51, 51),
            ["Grey-22"] = (56, 56, 56),
            ["Grey-24"] = (61, 61, 61),
            ["Grey-26"] = (66, 66, 66),
            ["Grey-28"] = (71, 71, 71),
            ["Grey-30"] = (77, 77, 77),
            ["Grey-32"] = (82, 82, 82),
            ["Grey-34"] = (87, 87, 87),
            ["Grey-36"] = (92, 92, 92),
            ["Grey-38"] = (97, 97, 97),
            ["Grey-4"] = (10, 10, 10),
            ["Grey-40"] = (102, 102, 102),
            ["Grey-42"] = (107, 107, 107),
            ["Grey-44"] = (112, 112, 112),
            ["Grey-46"] = (117, 117, 117),
            ["Grey-48"] = (122, 122, 122),
            ["Grey-50"] = (128, 128, 128),
            ["Grey-52"] = (133, 133, 133),
            ["Grey-54"] = (138, 138, 138),
            ["Grey-56"] = (143, 143, 143),
            ["Grey-58"] = (148, 148, 148),
            ["Grey-6"] = (15, 15, 15),
            ["Grey-60"] = (153, 153, 153),
            ["Grey-62"] = (158, 158, 158),
            ["Grey-64"] = (163, 163, 163),
            ["Grey-66"] = (168, 168, 168),
            ["Grey-68"] = (173, 173, 173),
            ["Grey-70"] = (179, 179, 179),
            ["Grey-72"] = (184, 184, 184),
            ["Grey-74"] = (189, 189, 189),
            ["Grey-76"] = (194, 194, 194),
            ["Grey-78"] = (199, 199, 199),
            ["Grey-8"] = (20, 20, 20),
            ["Grey-80"] = (204, 204, 204),
            ["Grey-82"] = (209, 209, 209),
            ["Grey-84"] = (214, 214, 214),
            ["Grey-86"] = (219, 219, 219),
            ["Grey-88"] = (224, 224, 224),
            ["Grey-90"] = (230, 230, 230),
            ["Grey-92"] = (235, 235, 235),
            ["Grey-94"] = (240, 240, 240),
            ["Grey-96"] = (245, 245, 245),
            ["Grey-98"] = (250, 250, 250),

            ["HotPink"] = (227, 0, 140),
            ["HotPink Shade 10"] = (204, 0, 126),
            ["HotPink Shade 20"] = (173, 0, 106),
            ["HotPink Shade 30"] = (127, 0, 78),
            ["HotPink Shade 40"] = (68, 0, 42),
            ["HotPink Shade 50"] = (36, 0, 22),
            ["HotPink Tint 10"] = (230, 28, 153),
            ["HotPink Tint 20"] = (234, 56, 166),
            ["HotPink Tint 30"] = (238, 95, 183),
            ["HotPink Tint 40"] = (247, 173, 218),
            ["HotPink Tint 50"] = (251, 210, 235),
            ["HotPink Tint 60"] = (254, 244, 250),

            ["Lavender"] = (113, 96, 232),
            ["Lavender Shade 10"] = (102, 86, 209),
            ["Lavender Shade 20"] = (86, 73, 176),
            ["Lavender Shade 30"] = (63, 54, 130),
            ["Lavender Shade 40"] = (34, 29, 70),
            ["Lavender Shade 50"] = (18, 15, 37),
            ["Lavender Tint 10"] = (129, 114, 235),
            ["Lavender Tint 20"] = (145, 132, 238),
            ["Lavender Tint 30"] = (167, 156, 241),
            ["Lavender Tint 40"] = (210, 204, 248),
            ["Lavender Tint 50"] = (231, 228, 251),
            ["Lavender Tint 60"] = (249, 248, 254),

            ["LightBlue"] = (58, 150, 221),
            ["LightBlue Shade 10"] = (52, 135, 199),
            ["LightBlue Shade 20"] = (44, 114, 168),
            ["LightBlue Shade 30"] = (32, 84, 124),
            ["LightBlue Shade 40"] = (17, 45, 66),
            ["LightBlue Shade 50"] = (9, 24, 35),
            ["LightBlue Tint 10"] = (79, 161, 225),
            ["LightBlue Tint 20"] = (101, 173, 229),
            ["LightBlue Tint 30"] = (131, 189, 235),
            ["LightBlue Tint 40"] = (191, 221, 245),
            ["LightBlue Tint 50"] = (220, 237, 250),
            ["LightBlue Tint 60"] = (246, 250, 254),

            ["LightGreen"] = (19, 161, 14),
            ["LightGreen Shade 10"] = (17, 145, 13),
            ["LightGreen Shade 20"] = (14, 122, 11),
            ["LightGreen Shade 30"] = (11, 90, 8),
            ["LightGreen Shade 40"] = (6, 48, 4),
            ["LightGreen Shade 50"] = (3, 26, 2),
            ["LightGreen Tint 10"] = (39, 172, 34),
            ["LightGreen Tint 20"] = (61, 184, 56),
            ["LightGreen Tint 30"] = (94, 199, 90),
            ["LightGreen Tint 40"] = (167, 227, 165),
            ["LightGreen Tint 50"] = (206, 240, 205),
            ["LightGreen Tint 60"] = (242, 251, 242),

            ["LightTeal"] = (0, 183, 195),
            ["LightTeal Shade 10"] = (0, 165, 175),
            ["LightTeal Shade 20"] = (0, 139, 148),
            ["LightTeal Shade 30"] = (0, 102, 109),
            ["LightTeal Shade 40"] = (0, 55, 58),
            ["LightTeal Shade 50"] = (0, 29, 31),
            ["LightTeal Tint 10"] = (24, 191, 202),
            ["LightTeal Tint 20"] = (50, 200, 209),
            ["LightTeal Tint 30"] = (88, 211, 219),
            ["LightTeal Tint 40"] = (166, 233, 237),
            ["LightTeal Tint 50"] = (206, 243, 245),
            ["LightTeal Tint 60"] = (242, 252, 253),

            ["Lilac"] = (177, 70, 194),
            ["Lilac Shade 10"] = (159, 63, 175),
            ["Lilac Shade 20"] = (134, 53, 147),
            ["Lilac Shade 30"] = (99, 39, 109),
            ["Lilac Shade 40"] = (53, 21, 58),
            ["Lilac Shade 50"] = (28, 11, 31),
            ["Lilac Tint 10"] = (186, 88, 201),
            ["Lilac Tint 20"] = (195, 107, 209),
            ["Lilac Tint 30"] = (207, 135, 218),
            ["Lilac Tint 40"] = (230, 191, 237),
            ["Lilac Tint 50"] = (242, 220, 245),
            ["Lilac Tint 60"] = (252, 246, 253),

            ["Lime"] = (115, 170, 36),
            ["Lime Shade 10"] = (104, 153, 32),
            ["Lime Shade 20"] = (87, 129, 27),
            ["Lime Shade 30"] = (64, 95, 20),
            ["Lime Shade 40"] = (35, 51, 11),
            ["Lime Shade 50"] = (18, 27, 6),
            ["Lime Tint 10"] = (129, 180, 55),
            ["Lime Tint 20"] = (144, 190, 76),
            ["Lime Tint 30"] = (164, 204, 108),
            ["Lime Tint 40"] = (207, 229, 175),
            ["Lime Tint 50"] = (229, 241, 211),
            ["Lime Tint 60"] = (248, 252, 244),

            ["Magenta"] = (191, 0, 119),
            ["Magenta Shade 10"] = (172, 0, 107),
            ["Magenta Shade 20"] = (145, 0, 90),
            ["Magenta Shade 30"] = (107, 0, 67),
            ["Magenta Shade 40"] = (57, 0, 36),
            ["Magenta Shade 50"] = (31, 0, 19),
            ["Magenta Tint 10"] = (199, 24, 133),
            ["Magenta Tint 20"] = (206, 50, 147),
            ["Magenta Tint 30"] = (217, 87, 168),
            ["Magenta Tint 40"] = (236, 165, 209),
            ["Magenta Tint 50"] = (245, 206, 230),
            ["Magenta Tint 60"] = (252, 242, 249),

            ["Marigold"] = (234, 163, 0),
            ["Marigold Shade 10"] = (211, 147, 0),
            ["Marigold Shade 20"] = (178, 124, 0),
            ["Marigold Shade 30"] = (131, 91, 0),
            ["Marigold Shade 40"] = (70, 49, 0),
            ["Marigold Shade 50"] = (37, 26, 0),
            ["Marigold Tint 10"] = (237, 173, 28),
            ["Marigold Tint 20"] = (239, 184, 57),
            ["Marigold Tint 30"] = (242, 198, 97),
            ["Marigold Tint 40"] = (249, 226, 174),
            ["Marigold Tint 50"] = (252, 239, 211),
            ["Marigold Tint 60"] = (254, 251, 244),

            ["Mink"] = (93, 90, 88),
            ["Mink Shade 10"] = (84, 81, 79),
            ["Mink Shade 20"] = (71, 68, 67),
            ["Mink Shade 30"] = (52, 50, 49),
            ["Mink Shade 40"] = (28, 27, 26),
            ["Mink Shade 50"] = (15, 14, 14),
            ["Mink Tint 10"] = (112, 109, 107),
            ["Mink Tint 20"] = (132, 129, 126),
            ["Mink Tint 30"] = (158, 155, 153),
            ["Mink Tint 40"] = (206, 204, 203),
            ["Mink Tint 50"] = (229, 228, 227),
            ["Mink Tint 60"] = (248, 248, 248),

            ["Navy"] = (0, 39, 180),
            ["Navy Shade 10"] = (0, 35, 162),
            ["Navy Shade 20"] = (0, 30, 137),
            ["Navy Shade 30"] = (0, 22, 101),
            ["Navy Shade 40"] = (0, 12, 54),
            ["Navy Shade 50"] = (0, 6, 29),
            ["Navy Tint 10"] = (23, 59, 189),
            ["Navy Tint 20"] = (48, 80, 198),
            ["Navy Tint 30"] = (84, 111, 210),
            ["Navy Tint 40"] = (163, 178, 232),
            ["Navy Tint 50"] = (204, 213, 243),
            ["Navy Tint 60"] = (242, 244, 252),

            ["Orange"] = (247, 99, 12),
            ["Orange Shade 10"] = (222, 89, 11),
            ["Orange Shade 20"] = (188, 75, 9),
            ["Orange Shade 30"] = (138, 55, 7),
            ["Orange Shade 40"] = (74, 30, 4),
            ["Orange Shade 50"] = (39, 16, 2),
            ["Orange Tint 10"] = (248, 117, 40),
            ["Orange Tint 20"] = (249, 136, 69),
            ["Orange Tint 30"] = (250, 160, 107),
            ["Orange Tint 40"] = (253, 207, 180),
            ["Orange Tint 50"] = (254, 229, 215),
            ["Orange Tint 60"] = (255, 249, 245),

            ["Orchid"] = (135, 100, 184),
            ["Orchid Shade 10"] = (121, 90, 166),
            ["Orchid Shade 20"] = (103, 76, 140),
            ["Orchid Shade 30"] = (76, 56, 103),
            ["Orchid Shade 40"] = (40, 30, 55),
            ["Orchid Shade 50"] = (22, 16, 29),
            ["Orchid Tint 10"] = (147, 115, 192),
            ["Orchid Tint 20"] = (160, 131, 201),
            ["Orchid Tint 30"] = (178, 154, 212),
            ["Orchid Tint 40"] = (215, 202, 234),
            ["Orchid Tint 50"] = (233, 226, 244),
            ["Orchid Tint 60"] = (249, 248, 252),

            ["Peach"] = (255, 140, 0),
            ["Peach Shade 10"] = (230, 126, 0),
            ["Peach Shade 20"] = (194, 106, 0),
            ["Peach Shade 30"] = (143, 78, 0),
            ["Peach Shade 40"] = (77, 42, 0),
            ["Peach Shade 50"] = (41, 22, 0),
            ["Peach Tint 10"] = (255, 154, 31),
            ["Peach Tint 20"] = (255, 168, 61),
            ["Peach Tint 30"] = (255, 186, 102),
            ["Peach Tint 40"] = (255, 221, 179),
            ["Peach Tint 50"] = (255, 237, 214),
            ["Peach Tint 60"] = (255, 250, 245),

            ["Pink"] = (228, 59, 166),
            ["Pink Shade 10"] = (205, 53, 149),
            ["Pink Shade 20"] = (173, 45, 126),
            ["Pink Shade 30"] = (128, 33, 93),
            ["Pink Shade 40"] = (68, 18, 50),
            ["Pink Shade 50"] = (36, 9, 27),
            ["Pink Tint 10"] = (231, 80, 176),
            ["Pink Tint 20"] = (234, 102, 186),
            ["Pink Tint 30"] = (239, 133, 200),
            ["Pink Tint 40"] = (247, 192, 227),
            ["Pink Tint 50"] = (251, 221, 240),
            ["Pink Tint 60"] = (254, 246, 251),

            ["Platinum"] = (105, 121, 126),
            ["Platinum Shade 10"] = (95, 109, 113),
            ["Platinum Shade 20"] = (80, 92, 96),
            ["Platinum Shade 30"] = (59, 68, 71),
            ["Platinum Shade 40"] = (31, 36, 38),
            ["Platinum Shade 50"] = (17, 19, 20),
            ["Platinum Tint 10"] = (121, 137, 141),
            ["Platinum Tint 20"] = (137, 152, 157),
            ["Platinum Tint 30"] = (160, 173, 178),
            ["Platinum Tint 40"] = (205, 214, 216),
            ["Platinum Tint 50"] = (228, 233, 234),
            ["Platinum Tint 60"] = (248, 249, 250),

            ["Plum"] = (119, 0, 77),
            ["Plum Shade 10"] = (107, 0, 69),
            ["Plum Shade 20"] = (90, 0, 59),
            ["Plum Shade 30"] = (67, 0, 43),
            ["Plum Shade 40"] = (36, 0, 23),
            ["Plum Shade 50"] = (19, 0, 12),
            ["Plum Tint 10"] = (135, 16, 93),
            ["Plum Tint 20"] = (152, 36, 111),
            ["Plum Tint 30"] = (173, 69, 137),
            ["Plum Tint 40"] = (214, 150, 192),
            ["Plum Tint 50"] = (233, 196, 220),
            ["Plum Tint 60"] = (250, 240, 246),

            ["Pumpkin"] = (202, 80, 16),
            ["Pumpkin Shade 10"] = (182, 72, 14),
            ["Pumpkin Shade 20"] = (154, 61, 12),
            ["Pumpkin Shade 30"] = (113, 45, 9),
            ["Pumpkin Shade 40"] = (61, 24, 5),
            ["Pumpkin Shade 50"] = (32, 13, 3),
            ["Pumpkin Tint 10"] = (208, 98, 40),
            ["Pumpkin Tint 20"] = (215, 116, 64),
            ["Pumpkin Tint 30"] = (223, 142, 100),
            ["Pumpkin Tint 40"] = (239, 196, 173),
            ["Pumpkin Tint 50"] = (247, 223, 210),
            ["Pumpkin Tint 60"] = (253, 247, 244),

            ["Purple"] = (92, 46, 145),
            ["Purple Shade 10"] = (83, 41, 130),
            ["Purple Shade 20"] = (70, 35, 110),
            ["Purple Shade 30"] = (52, 26, 81),
            ["Purple Shade 40"] = (28, 14, 43),
            ["Purple Shade 50"] = (15, 7, 23),
            ["Purple Tint 10"] = (107, 63, 158),
            ["Purple Tint 20"] = (124, 82, 171),
            ["Purple Tint 30"] = (148, 112, 189),
            ["Purple Tint 40"] = (198, 177, 222),
            ["Purple Tint 50"] = (224, 211, 237),
            ["Purple Tint 60"] = (247, 244, 251),

            ["Red"] = (209, 52, 56),
            ["Red Shade 10"] = (188, 47, 50),
            ["Red Shade 20"] = (159, 40, 43),
            ["Red Shade 30"] = (117, 29, 31),
            ["Red Shade 40"] = (63, 16, 17),
            ["Red Shade 50"] = (33, 8, 9),
            ["Red Tint 10"] = (215, 73, 76),
            ["Red Tint 20"] = (220, 94, 98),
            ["Red Tint 30"] = (227, 125, 128),
            ["Red Tint 40"] = (241, 187, 188),
            ["Red Tint 50"] = (248, 218, 219),
            ["Red Tint 60"] = (253, 246, 246),

            ["RoyalBlue"] = (0, 78, 140),
            ["RoyalBlue Shade 10"] = (0, 70, 126),
            ["RoyalBlue Shade 20"] = (0, 59, 106),
            ["RoyalBlue Shade 30"] = (0, 44, 78),
            ["RoyalBlue Shade 40"] = (0, 23, 42),
            ["RoyalBlue Shade 50"] = (0, 12, 22),
            ["RoyalBlue Tint 10"] = (18, 94, 154),
            ["RoyalBlue Tint 20"] = (40, 111, 168),
            ["RoyalBlue Tint 30"] = (74, 137, 186),
            ["RoyalBlue Tint 40"] = (154, 191, 220),
            ["RoyalBlue Tint 50"] = (199, 220, 237),
            ["RoyalBlue Tint 60"] = (240, 246, 250),

            ["Seafoam"] = (0, 204, 106),
            ["Seafoam Shade 10"] = (0, 184, 95),
            ["Seafoam Shade 20"] = (0, 155, 81),
            ["Seafoam Shade 30"] = (0, 114, 59),
            ["Seafoam Shade 40"] = (0, 61, 32),
            ["Seafoam Shade 50"] = (0, 33, 17),
            ["Seafoam Tint 10"] = (25, 210, 121),
            ["Seafoam Tint 20"] = (52, 216, 137),
            ["Seafoam Tint 30"] = (90, 224, 160),
            ["Seafoam Tint 40"] = (168, 240, 205),
            ["Seafoam Tint 50"] = (207, 247, 228),
            ["Seafoam Tint 60"] = (243, 253, 248),

            ["Silver"] = (133, 149, 153),
            ["Silver Shade 10"] = (120, 134, 138),
            ["Silver Shade 20"] = (101, 113, 116),
            ["Silver Shade 30"] = (74, 83, 86),
            ["Silver Shade 40"] = (40, 45, 46),
            ["Silver Shade 50"] = (21, 24, 24),
            ["Silver Tint 10"] = (146, 161, 165),
            ["Silver Tint 20"] = (160, 174, 177),
            ["Silver Tint 30"] = (179, 191, 194),
            ["Silver Tint 40"] = (216, 223, 224),
            ["Silver Tint 50"] = (234, 238, 239),
            ["Silver Tint 60"] = (250, 251, 251),

            ["Steel"] = (0, 91, 112),
            ["Steel Shade 10"] = (0, 82, 101),
            ["Steel Shade 20"] = (0, 69, 85),
            ["Steel Shade 30"] = (0, 51, 63),
            ["Steel Shade 40"] = (0, 27, 34),
            ["Steel Shade 50"] = (0, 15, 18),
            ["Steel Tint 10"] = (15, 108, 129),
            ["Steel Tint 20"] = (35, 125, 146),
            ["Steel Tint 30"] = (68, 150, 169),
            ["Steel Tint 40"] = (148, 200, 212),
            ["Steel Tint 50"] = (195, 225, 232),
            ["Steel Tint 60"] = (239, 247, 249),

            ["Teal"] = (3, 131, 135),
            ["Teal Shade 10"] = (3, 118, 121),
            ["Teal Shade 20"] = (2, 100, 103),
            ["Teal Shade 30"] = (2, 73, 76),
            ["Teal Shade 40"] = (1, 39, 40),
            ["Teal Shade 50"] = (0, 21, 22),
            ["Teal Tint 10"] = (21, 145, 149),
            ["Teal Tint 20"] = (42, 160, 164),
            ["Teal Tint 30"] = (76, 180, 183),
            ["Teal Tint 40"] = (155, 217, 219),
            ["Teal Tint 50"] = (199, 235, 236),
            ["Teal Tint 60"] = (240, 250, 250),

            ["White"] = (255, 255, 255),

            ["Yellow"] = (253, 227, 0),
            ["Yellow Shade 10"] = (228, 204, 0),
            ["Yellow Shade 20"] = (192, 173, 0),
            ["Yellow Shade 30"] = (129, 116, 0),
            ["Yellow Shade 40"] = (76, 68, 0),
            ["Yellow Shade 50"] = (40, 36, 0),
            ["Yellow Tint 10"] = (253, 230, 30),
            ["Yellow Tint 20"] = (253, 234, 61),
            ["Yellow Tint 30"] = (254, 238, 102),
            ["Yellow Tint 40"] = (254, 247, 178),
            ["Yellow Tint 50"] = (255, 250, 214),
            ["Yellow Tint 60"] = (255, 254, 245),


            // Semantic Colors
            ["success"] = (49, 140, 54),
            ["warning"] = (255, 144, 0),
            ["danger"] = (164, 38, 44),
            ["info"] = (0, 99, 177),

            // Microsoft Brand Colors
            ["microsoft blue"] = (0, 103, 184),
            ["microsoft green"] = (16, 124, 16),
            ["microsoft red"] = (232, 17, 35),
            ["microsoft yellow"] = (255, 185, 0),

            // Office App Colors
            ["excel green"] = (16, 124, 16),
            ["word blue"] = (43, 87, 154),
            ["powerpoint orange"] = (183, 71, 42),
            ["outlook blue"] = (0, 114, 198),
            ["teams purple"] = (75, 59, 160),
            ["onenote purple"] = (128, 57, 123),
            ["sharepoint blue"] = (0, 120, 212),
            ["yammer blue"] = (0, 115, 178),
            ["sway green"] = (0, 158, 97),
            ["delve orange"] = (219, 129, 15)
        };



    public string Name => "Fluent colors";

    public string Id => "fluent";

    public IReadOnlyDictionary<string, RgbColor> Colors => this._colors;
}