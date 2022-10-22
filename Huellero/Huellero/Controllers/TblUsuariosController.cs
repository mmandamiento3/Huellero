using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Huellero.Models;
using SecuGen.FDxSDKPro.Windows;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Components.Web;
using System.Drawing;

namespace Huellero.Controllers
{
    public class TblUsuariosController : Controller
    {
        private readonly PruebaContext _context;
        private SGFingerPrintManager m_FPM;
        //private bool m_LedOn = false;
        private Int32 m_ImageWidth;
        private Int32 m_ImageHeight;
        //private Byte[] m_RegMin1 = null;
        //private Byte[] m_RegMin2;
        //private Byte[] m_VrfMin;
        private SGFPMDeviceList[] m_DevList;
        List<SelectListItem> ListaDispositivos;
        public TblUsuariosController(PruebaContext context)
        {
            _context = context;

            m_FPM = new SGFingerPrintManager();
            Listar_Dispositivos();
            GenerarHuella();
            
        }
         private List<SelectListItem> Listar_Dispositivos()
        {
            Int32 iError;
            string enum_device;
            

            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];
           ListaDispositivos = new List<SelectListItem>();

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;

                ListaDispositivos.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = enum_device
                });

               
            }

      
            return ListaDispositivos;
        }




        // GET: TblUsuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.TblUsuarios.ToListAsync());
        }

        // GET: TblUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // GET: TblUsuarios/Create
        public IActionResult Create()
        {

            Int32 iError;
            string enum_device;


            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];
            ListaDispositivos = new List<SelectListItem>();

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;

                ListaDispositivos.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = enum_device
                });


            }
            ViewBag.Dispositivoss = ListaDispositivos;
            return View();
        }

        // POST: TblUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario == null)
            {
                return NotFound();
            }
            return View(tblUsuario);
        }

        // POST: TblUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (id != tblUsuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUsuarioExists(tblUsuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // POST: TblUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUsuarios == null)
            {
                return Problem("Entity set 'PruebaContext.TblUsuarios'  is null.");
            }
            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario != null)
            {
                _context.TblUsuarios.Remove(tblUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private bool TblUsuarioExists(int id)
        {
          return _context.TblUsuarios.Any(e => e.Id == id);
        }

        //public static Task<JsonResult> Prueba(int idSeleccionado)
        //{
        //    //return  Json(AbrirDispositivo(idSeleccionado));
        //}

        [HttpPost]
        public async Task<JsonResult> AbrirDispositivo(int ValorSeleccionado)
        {
            if (m_FPM.NumberOfDevice == 0)
                return Json(6);

            SGFPMDeviceName device_name;
            int device_id;
            int iError;      
         
            device_name = m_DevList[ValorSeleccionado].DevName;
            device_id = m_DevList[ValorSeleccionado].DevID;

            iError = OpenDevice2(device_name, device_id);
            return Json(iError);
        }


        private int OpenDevice2(SGFPMDeviceName device_name, int device_id)
        {
            int iError = m_FPM.Init(device_name);
            iError = m_FPM.OpenDevice(device_id);
            

           return iError;
        }

        private void Obtener()
        {
            SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
            int iError = m_FPM.GetDeviceInfo(pInfo);

            if (iError == (int)SGFPMError.ERROR_NONE)
            {
                m_ImageWidth = pInfo.ImageWidth;
                m_ImageHeight = pInfo.ImageHeight;
            }
        }
        private void GenerarHuella()
        {
            int iError;
            int elap_time;
            Byte[] fp_image;
            //Cursor.Current = Cursors.WaitCursor;

            elap_time = Environment.TickCount;
            fp_image = new Byte[m_ImageWidth * m_ImageHeight];

            iError = m_FPM.GetImage(fp_image);


            if (iError == (int)SGFPMError.ERROR_NONE)
            {
                elap_time = Environment.TickCount - elap_time;
                DrawImage(fp_image);
                //StatusBar.Text = "Capture Time : " + elap_time + " ms";
            }
            
        }

        private void DrawImage(Byte[] imgData)//, PictureBox picBox)
        {
            int colorval;
            Bitmap bmp = new Bitmap(m_ImageWidth, m_ImageHeight);
            //picBox.Image = (Image)bmp;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    colorval = (int)imgData[(j * m_ImageWidth) + i];
                    bmp.SetPixel(i, j, Color.FromArgb(colorval, colorval, colorval));
                }
            }
            //picBox.Refresh();
        }



    }
}
