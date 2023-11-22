using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

string tipoComprobante = null;

while (true) {

    try
    {
        Console.Write("Por favor, escriba 'recibo' o 'factura': ");
        tipoComprobante = Console.ReadLine().ToLower();

        if (tipoComprobante != "recibo" && tipoComprobante != "factura")
        {
            throw new InvalidOperationException("Entrada inválida. Debe escribir 'recibo' o 'factura'.\n");

        }
        break;
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}


if (tipoComprobante == "recibo")
{   
    Console.WriteLine("---------------------------------------------------------");
    recibo();
}
else
{
    Console.WriteLine("---------------------------------------------------------");
    factura();

}

void recibo()
{
    
    Console.Write("Ingrese los detalles del pago: ");
    string detallesPago = Console.ReadLine();
    Console.Write("Ingrese el nombre del cliente: ");
    string nombre = Console.ReadLine();
    Console.Write("Ingrese el teléfono del cliente: ");
    string telefono = Console.ReadLine();
    Console.Write("Ingrese el nombre del representante legal: ");
    string representante = Console.ReadLine();

    List<string> listNombreProducto = new List<string>();
    List<float> listPrecioProducto = new List<float>();
    List<int> listCantidadProducto = new List<int>();

    int A = 0;

    while (true)
    {
        A++;
        Console.WriteLine("---------------------------------------------------------");
        Console.Write($"Ingrese el producto #{A} o presione la tecla enter: ");
        string producto = Console.ReadLine().ToLower();

        if (string.IsNullOrWhiteSpace(producto))
        {
            Console.WriteLine("---------------------------------------------------------");
            break;
        }
        listNombreProducto.Add(producto);


        float precioProducto;
        while (true)
        {
            Console.Write($"Ingrese el precio del producto #{A} a comprar: COP$");
            string input = Console.ReadLine();

            if (float.TryParse(input, out precioProducto))
            {
                if (precioProducto % 1 == 0)
                {
                   
                    int precioProductoEntero = (int)precioProducto;
                    listPrecioProducto.Add(precioProductoEntero);
                }
                else
                {
                    listPrecioProducto.Add(precioProducto);
                }
                break;
            }
            else
            {
                Console.WriteLine("Error: ingrese un valor numérico");
            }
        }

        int cantidadProducto;
        while (true)
        {
            Console.Write($"Ingrese la cantidad del producto #{A} a comprar: ");
            if (int.TryParse(Console.ReadLine(), out cantidadProducto))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: ingrese un valor numérico entero");
            }
        }
        listCantidadProducto.Add(cantidadProducto);
    }



    Document doc = new Document();

    string descargasPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    string nombreDirectorio = Path.Combine(descargasPath, "Downloads", "Recibos");


    if (!Directory.Exists(nombreDirectorio))
    {
        Directory.CreateDirectory(nombreDirectorio);
    }

    string nombreArchivo = $"{nombreDirectorio}\\Recibo_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf";
    PdfWriter.GetInstance(doc, new FileStream(nombreArchivo, FileMode.Create));

    StringBuilder reciboContent = new StringBuilder();

    doc.Open();

    float ImprimirRecibo(string detallesPago, string nombre,
string telefono, string representante, List<string> listNombreProducto, List<float> listPrecioProducto,
List<int> listCantidadProducto, StringBuilder reciboContent)
    {
        reciboContent.AppendLine("---------------------------------------------------------");
        reciboContent.AppendLine("RECIBO SHORT");
        reciboContent.AppendLine("---------------------------------------------------------");
        reciboContent.AppendLine("Medical Printer");
        reciboContent.AppendLine("---------------------------------------------------------");
        reciboContent.AppendLine($"Fecha: {DateTime.Now:yyyy/MM/dd}");
        reciboContent.AppendLine($"Código: {DateTime.Now:HHmmss}");
        reciboContent.AppendLine($"Detalles del pago: {detallesPago}");
        reciboContent.AppendLine($"Nombre: {nombre}");
        reciboContent.AppendLine($"Teléfono: {telefono}");
        reciboContent.AppendLine($"Representante legal: {representante}");
        reciboContent.AppendLine("---------------------------------------------------------");

        int A = 0;
        for (int i = 0; i < listNombreProducto.Count; i++)
        {
            A++;
            reciboContent.AppendLine($"Producto #{A}: {listNombreProducto[i]}");
            reciboContent.AppendLine($"Precio del producto #{A}: COP${listPrecioProducto[i]}");
            reciboContent.AppendLine($"Cantidad del producto #{A}: {listCantidadProducto[i]}");
            reciboContent.AppendLine("---------------------------------------------------------");

        }

        List<float> monto = new List<float>();

        for (int i = 0; i < listPrecioProducto.Count; i++)
        {
            float multiplicacion = listPrecioProducto[i] * listCantidadProducto[i];
            monto.Add(multiplicacion);
        }

        float total = 0;
        foreach (float B in monto)
        {
            total += B;
        }

        return total;

    }


    float total1 = ImprimirRecibo(detallesPago, nombre, telefono, representante,
        listNombreProducto, listPrecioProducto, listCantidadProducto, reciboContent);

    if (total1 > 90000)
    {
        total1 = 90000;
        reciboContent.AppendLine($"Total: COP${total1}");
        reciboContent.AppendLine("---------------------------------------------------------");

        doc.Add(new Paragraph(reciboContent.ToString()));
        reciboContent.Clear();

        doc.NewPage();

        float total2 = ImprimirRecibo(detallesPago, nombre, telefono, representante,
            listNombreProducto, listPrecioProducto, listCantidadProducto, reciboContent);

        total2 -= total1;
        reciboContent.AppendLine($"Total: COP${total2}");
        reciboContent.AppendLine("---------------------------------------------------------");

    }

    doc.Add(new Paragraph(reciboContent.ToString()));
    doc.Close();
}


void factura()
{
    Console.Write("Ingrese el nombre del cliente: ");
    string nombre = Console.ReadLine();
    Console.Write("Ingrese la dirección del cliente: ");
    string direccion = Console.ReadLine();
    Console.Write("Ingrese el número de cédula del cliente: ");
    string cedula = Console.ReadLine();
    Console.Write("Ingrese el número de teléfono del cliente: ");
    string telefono = Console.ReadLine();
    Console.Write("Ingrese el correo electrónico del cliente: ");
    string email = Console.ReadLine();
    Console.Write("Ingrese el nombre del representante legal: ");
    string representante = Console.ReadLine();

    List<string> listNombreProducto = new List<string>();
    List<float> listPrecioProducto = new List<float>();
    List<int> listCantidadProducto = new List<int>();

    int A = 0;
    while (true)
    {
        A++;
        Console.WriteLine("---------------------------------------------------------");
        Console.Write($"Ingrese el producto #{A} o presione la tecla enter: ");
        string producto = Console.ReadLine().ToLower();

        if (string.IsNullOrWhiteSpace(producto))
        {
            Console.WriteLine("---------------------------------------------------------");
            break;
        }

        listNombreProducto.Add(producto);

        float precioProducto;
        while (true)
        {
            Console.Write($"Ingrese el precio del producto #{A}: COP$");
            if (float.TryParse(Console.ReadLine(), out precioProducto))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: ingrese un valor numérico");
            }
        }
        listPrecioProducto.Add(precioProducto);

        int cantidadProducto;
        while (true)
        {
            Console.Write($"Ingrese la cantidad del producto #{A} a comprar: ");
            if (int.TryParse(Console.ReadLine(), out cantidadProducto))
            {
                break;
            }
            else
            {
                Console.WriteLine("Error: ingrese un valor numérico");
            }
        }
        listCantidadProducto.Add(cantidadProducto);

    }


    Document doc = new Document();

    string descargasPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    string nombreDirectorio = Path.Combine(descargasPath, "Downloads", "Facturas");

    if (!Directory.Exists(nombreDirectorio))
    {
        Directory.CreateDirectory(nombreDirectorio);
    }

    string nombreArchivo = $"{nombreDirectorio}\\Factura_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf";
    PdfWriter.GetInstance(doc, new FileStream(nombreArchivo, FileMode.Create));

    StringBuilder facturaContent = new StringBuilder();

    doc.Open();

    facturaContent.AppendLine("---------------------------------------------------------");
    facturaContent.AppendLine("FACTURA");
    facturaContent.AppendLine("---------------------------------------------------------");
    facturaContent.AppendLine("Medical Printer");
    facturaContent.AppendLine("---------------------------------------------------------");
    facturaContent.AppendLine($"Fecha de compra: {DateTime.Now:yyyy/MM/dd}");
    facturaContent.AppendLine($"Código: {DateTime.Now:HHmmss}");
    facturaContent.AppendLine($"Nombre del cliente: {nombre}");
    facturaContent.AppendLine($"Direccion: {direccion}");
    facturaContent.AppendLine($"Número de cedula: {cedula}");
    facturaContent.AppendLine($"Teléfono: {telefono}");
    facturaContent.AppendLine($"Correo electronico: {email}");
    facturaContent.AppendLine("Firma del comprador: ___________");
    facturaContent.AppendLine($"Representante legal: {representante}");
    facturaContent.AppendLine("---------------------------------------------------------");

    int J = 0;
    for (int i = 0; i < listNombreProducto.Count; i++)
    {
        J++;
        facturaContent.AppendLine($"Producto #{J}: {listNombreProducto[i]}");
        facturaContent.AppendLine($"Precio del producto #{J}: COP${listPrecioProducto[i]}");
        facturaContent.AppendLine($"Cantidad del producto #{J} a comprar: {listCantidadProducto[i]}");
        facturaContent.AppendLine("---------------------------------------------------------");
    }

    List<float> monto = new List<float>();
    for (int i = 0; i < listPrecioProducto.Count; i++)
    {
        float multiplicacion = listPrecioProducto[i] * listCantidadProducto[i];
        monto.Add(multiplicacion);
    }

    float subtotal = 0;
    foreach (float B in monto)
    {
        subtotal += B;
    }
    facturaContent.AppendLine($"Subtotal: COP${subtotal}");
    facturaContent.AppendLine("---------------------------------------------------------");

    float iva = 1.10f;
    float subtotalIva = subtotal * iva;
    facturaContent.AppendLine($"IVA: COP${subtotalIva}");
    facturaContent.AppendLine("---------------------------------------------------------");

    float total = subtotalIva;

    if (subtotalIva >= 1000000 && subtotalIva < 2000000)
    {
        float subtotalDescuento = subtotalIva - (subtotalIva * 0.10f);
        facturaContent.AppendLine($"Descuento 10%: COP${subtotalDescuento}");
        facturaContent.AppendLine("---------------------------------------------------------");
        total = subtotalDescuento;
    }

    if (subtotalIva > 2000000)
    {
        float subtotalDescuento = subtotalIva - (subtotalIva * 0.20f);
        facturaContent.AppendLine($"Descuento 20%: COP${subtotalDescuento}");
        facturaContent.AppendLine("---------------------------------------------------------");
        total = subtotalDescuento;
    }
    facturaContent.AppendLine($"Total: COP${total}");
    facturaContent.AppendLine("---------------------------------------------------------");


    doc.Add(new Paragraph(facturaContent.ToString()));
    doc.Close();

}