-- ============================================================
--  PROYECTO: El Ahorcado (multijugador)
--  Script 2 de 2: DATOS (relleno de las tablas)
--  Ejecutar DESPUÉS de 01_esquema_ahorcado.sql
-- ============================================================

USE ahorcado_juego;
GO

-- =====================
-- Estado (6 estados que cubren todos los desenlaces)
-- =====================
INSERT INTO Estado VALUES (1, 'En espera', 'Waiting');
INSERT INTO Estado VALUES (2, 'En curso', 'In progress');
INSERT INTO Estado VALUES (3, 'Ganó el adivinador', 'Guesser won');
INSERT INTO Estado VALUES (4, 'Ganó el creador', 'Creator won');
INSERT INTO Estado VALUES (5, 'Abandonada por creador', 'Abandoned by creator');
INSERT INTO Estado VALUES (6, 'Abandonada por adivinador', 'Abandoned by guesser');

-- =====================
-- Idioma
-- =====================
INSERT INTO Idioma (NombreIdioma) VALUES ('Español');
INSERT INTO Idioma (NombreIdioma) VALUES ('English');

-- =====================
-- Categoria
-- =====================
INSERT INTO Categoria (CategoriaES, CategoriaEN) VALUES ('Frutas y Verduras', 'Fruits and Vegetables');
INSERT INTO Categoria (CategoriaES, CategoriaEN) VALUES ('Animales', 'Animals');
INSERT INTO Categoria (CategoriaES, CategoriaEN) VALUES ('Países', 'Countries');
INSERT INTO Categoria (CategoriaES, CategoriaEN) VALUES ('Deportes', 'Sports');
INSERT INTO Categoria (CategoriaES, CategoriaEN) VALUES ('Tecnología', 'Technology');

-- =====================
-- Palabra
-- =====================
-- Frutas y Verduras (CategoriaId = 1)
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Naranja', 'Orange', 'Fruta cítrica de color anaranjado rica en vitamina C.', 'Citrus fruit with an orange color rich in vitamin C.', 1);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Limon', 'Lemon', 'Fruta ácida de color amarillo usada en bebidas y comidas.', 'Yellow acidic fruit used in drinks and food.', 1);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Platano', 'Banana', 'Fruta alargada de color amarillo rica en potasio.', 'Long yellow fruit rich in potassium.', 1);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Papaya', 'Papaya', 'Fruta tropical de pulpa anaranjada y sabor dulce.', 'Tropical fruit with orange flesh and sweet flavor.', 1);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Mango', 'Mango', 'Fruta tropical jugosa de sabor dulce y aroma intenso.', 'Juicy tropical fruit with sweet taste and intense aroma.', 1);

-- Animales (CategoriaId = 2)
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Elefante', 'Elephant', 'El animal terrestre más grande del mundo con una larga trompa.', 'The largest land animal in the world with a long trunk.', 2);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Jirafa', 'Giraffe', 'Animal africano con el cuello más largo de todos los mamíferos.', 'African animal with the longest neck of all mammals.', 2);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Delfin', 'Dolphin', 'Mamífero marino muy inteligente conocido por su agilidad.', 'Very intelligent marine mammal known for its agility.', 2);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Aguila', 'Eagle', 'Ave rapaz de gran tamaño conocida por su vista aguda.', 'Large bird of prey known for its sharp eyesight.', 2);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Tiburon', 'Shark', 'Pez depredador marino con hileras de dientes afilados.', 'Marine predatory fish with rows of sharp teeth.', 2);

-- Países (CategoriaId = 3)
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Mexico', 'Mexico', 'País de América del Norte famoso por sus pirámides y tacos.', 'North American country famous for its pyramids and tacos.', 3);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Francia', 'France', 'País europeo famoso por la Torre Eiffel y su gastronomía.', 'European country famous for the Eiffel Tower and its cuisine.', 3);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Japon', 'Japan', 'País asiático insular conocido por su tecnología y cultura.', 'Island Asian country known for its technology and culture.', 3);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Brasil', 'Brazil', 'País más grande de América del Sur famoso por el carnaval.', 'Largest country in South America famous for carnival.', 3);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Canada', 'Canada', 'País del norte de América conocido por sus grandes paisajes nevados.', 'North American country known for its large snowy landscapes.', 3);

-- Deportes (CategoriaId = 4)
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Futbol', 'Soccer', 'Deporte más popular del mundo jugado con un balón redondo.', 'Most popular sport in the world played with a round ball.', 4);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Tenis', 'Tennis', 'Deporte de raqueta jugado en una cancha dividida por una red.', 'Racket sport played on a court divided by a net.', 4);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Natacion', 'Swimming', 'Deporte acuático que consiste en desplazarse en el agua.', 'Water sport that consists of moving through the water.', 4);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Ciclismo', 'Cycling', 'Deporte que consiste en recorrer distancias en bicicleta.', 'Sport that consists of covering distances by bicycle.', 4);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Beisbol', 'Baseball', 'Deporte de bate y guante muy popular en América y el Caribe.', 'Bat and glove sport very popular in America and the Caribbean.', 4);

-- Tecnología (CategoriaId = 5)
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Computadora', 'Computer', 'Dispositivo electrónico capaz de procesar y almacenar información.', 'Electronic device capable of processing and storing information.', 5);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Teclado', 'Keyboard', 'Dispositivo de entrada que permite escribir texto en una computadora.', 'Input device that allows typing text on a computer.', 5);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Monitor', 'Monitor', 'Pantalla que muestra la interfaz visual de una computadora.', 'Screen that displays the visual interface of a computer.', 5);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Servidor', 'Server', 'Computadora que provee servicios y recursos a otros dispositivos.', 'Computer that provides services and resources to other devices.', 5);
INSERT INTO Palabra (PalabraES, PalabraEN, DescripcionES, DescripcionEN, CategoriaId)
VALUES ('Internet', 'Internet', 'Red global de computadoras interconectadas en todo el mundo.', 'Global network of interconnected computers around the world.', 5);

-- =====================
-- Jugador
-- Contraseña de TODOS los usuarios de prueba: Ahorcado2026
-- (los hashes son BCrypt reales, compatibles con BCrypt.Net del servidor)
-- =====================
INSERT INTO Jugador (Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono, Contrasena, Correo, Puntos, Usuario)
VALUES ('Carlos', 'Ramirez', 'Lopez', '2000-03-15', '2281234567', '$2b$11$V861mbIpAzfzuTq1NseRbuDUV/1gbqIwvrWOZ1XtwcNJ.PM79j7uW', 'carlos.ramirez@gmail.com', 0, 'carlitos00');
INSERT INTO Jugador (Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono, Contrasena, Correo, Puntos, Usuario)
VALUES ('Maria', 'Hernandez', 'Torres', '1999-07-22', '2289876543', '$2b$11$rZ/6BR7uCYrKoo7HPKannuuBVTBEVovsPuWRpWjl.e.EX/dSSxnc.', 'maria.hernandez@gmail.com', 0, 'mariahtz');
INSERT INTO Jugador (Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono, Contrasena, Correo, Puntos, Usuario)
VALUES ('Javier', 'Morales', 'Sanchez', '2001-11-05', '2284567890', '$2b$11$R63BFg0pXkWBRaBqtdsRSO23W.AxbEGwe1uINl8XQscBhztzrTT1u', 'javier.morales@gmail.com', 0, 'javimor01');
INSERT INTO Jugador (Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono, Contrasena, Correo, Puntos, Usuario)
VALUES ('Ana', 'Garcia', 'Perez', '2000-01-30', '2283456789', '$2b$11$swHdlLrVaoB/Gy0ztbfsDuAEQVpi3u2em5WySa01hZqQo3hH5Op02', 'ana.garcia@gmail.com', 0, 'anagp00');
INSERT INTO Jugador (Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono, Contrasena, Correo, Puntos, Usuario)
VALUES ('Luis', 'Martinez', NULL, '1998-05-18', '2282345678', '$2b$11$k23rwmnCL0EZS0t2NT13weDWkodRT5oZlLa.wjTomCTTAo25/MtmK', 'luis.martinez@gmail.com', 0, 'luismtz98');

-- =====================
-- Partida (ejemplos en distintos estados; sirven para probar el historial y el ranking)
-- =====================
-- En espera
INSERT INTO Partida (NombrePartida, FechaCreacion, FechaFin, EstadoId, JugadorAId, JugadorBId, PalabraId, IdiomaId)
VALUES ('PartidaDeCarlos', '2026-06-01 10:00:00', NULL, 1, 1, NULL, 3, 1);
-- En curso
INSERT INTO Partida (NombrePartida, FechaCreacion, FechaFin, EstadoId, JugadorAId, JugadorBId, PalabraId, IdiomaId)
VALUES ('RetoAnaMaria', '2026-06-02 14:30:00', NULL, 2, 4, 2, 7, 1);
-- Ganó el adivinador
INSERT INTO Partida (NombrePartida, FechaCreacion, FechaFin, EstadoId, JugadorAId, JugadorBId, PalabraId, IdiomaId)
VALUES ('BatallaFinal', '2026-06-03 09:00:00', '2026-06-03 09:25:00', 3, 3, 1, 12, 2);
-- Ganó el creador
INSERT INTO Partida (NombrePartida, FechaCreacion, FechaFin, EstadoId, JugadorAId, JugadorBId, PalabraId, IdiomaId)
VALUES ('DesafioJavier', '2026-06-03 16:00:00', '2026-06-03 16:40:00', 4, 3, 5, 18, 1);
-- Abandonada por adivinador
INSERT INTO Partida (NombrePartida, FechaCreacion, FechaFin, EstadoId, JugadorAId, JugadorBId, PalabraId, IdiomaId)
VALUES ('PartidaRapida', '2026-06-04 11:00:00', '2026-06-04 11:10:00', 6, 2, 4, 22, 1);
GO
